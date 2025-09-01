import http from 'k6/http';
import { check, sleep } from 'k6';
import { SharedArray } from 'k6/data';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

const BASE_URL = 'https://quickpizza.grafana.com';

export function handleSummary(data) {
    const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
    return {
        [`results/my_messages_${timestamp}.html`]: htmlReport(data),
    };
}

const users = new SharedArray("usuarios", function () {
    return open('../TestData/usuarios.csv')
        .split('\n')
        .slice(1)
        .filter(line => line.trim() !== '')
        .map(line => {
            const [login, password] = line.split(',');
            return { login, password };
        });
});

export const options = {
    stages: [
        { duration: '30s', target: 100 },
        { duration: '1m', target: 500 },
        { duration: '1m', target: 1000 },
        { duration: '30s', target: 0 },
    ],
    thresholds: {
        http_req_duration: ['p(95)<1000'],
        http_req_failed: ['rate<0.05'],
    },
};

export default function () {
    const user = users[__VU % users.length];

    const res = http.post(`${BASE_URL}/my_messages.php`, {
        login: user.login,
        password: user.password,
    });

    // Após login, acessa /admin.php diretamente
    const adminPage = http.get(`${BASE_URL}/admin.php`);

    check(adminPage, {
        'admin page status 200': (r) => r && r.status === 200,
        'contains Welcome': (r) => r && r.body && r.body.includes('Welcome'),
        'contains Logout button': (r) => r && r.body && r.body.includes('Logout'),
    });

    sleep(1);
}
