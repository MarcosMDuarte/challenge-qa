import http from 'k6/http';
import { check, sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

const BASE_URL = 'https://quickpizza.grafana.com';

export function handleSummary(data) {
    const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
    return {
        [`results/flip_coin_test_result${timestamp}.html`]: htmlReport(data),
    };
}

export const options = {
    scenarios: {
        carga_100: {
            executor: 'constant-vus',
            vus: 100,
            duration: '30s',
            exec: 'flipCoin',    
        },
        carga_500: {
            executor: 'constant-vus',
            vus: 500,
            duration: '30s',
            startTime: '40s',
            exec: 'flipCoin',
        },
        carga_1000: {
            executor: 'constant-vus',
            vus: 1000,
            duration: '30s',
            startTime: '1m20s',
            exec: 'flipCoin', 
        },
    },
    thresholds: {
        'http_req_duration{scenario:carga_100}': ['p(95)<1000'],
        'http_req_duration{scenario:carga_500}': ['p(95)<1000'],
        'http_req_duration{scenario:carga_1000}': ['p(95)<1000'],
        http_req_failed: ['rate<0.05'],
    }
};

export function flipCoin() {
    const resHeads = http.get(`${BASE_URL}/flip_coin.php?bet=heads`);
    check(resHeads, { 'flip_coin HEADS 200': (r) => r.status === 200 });

    const resTails = http.get(`${BASE_URL}/flip_coin.php?bet=tails`);
    check(resTails, { 'flip_coin TAILS 200': (r) => r.status === 200 });

    sleep(1);
}

