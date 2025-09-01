# Estratégia de Massa de Dados e Configuração de Testes (k6-data.md)

## Introdução ao k6

[k6](https://k6.io) é uma ferramenta open-source para testes de carga e performance, escrita em Go e com scripts em JavaScript. Ela permite simular usuários virtuais (**VUs**) acessando sistemas de forma concorrente, avaliando métricas como latência, throughput, taxa de falhas e consumo de recursos.

No projeto, utilizei o k6 para validar os endpoints `/flip_coin.php` e `/my_messages.php`, criando cenários de 100, 500 e 1000 usuários simultâneos.


## Objetivo

Garantir que os testes de carga simulem usuários reais, com variação de dados e cenários.

## Estratégia

* **flip\_coin.php**

  * Cada usuário virtual realiza duas apostas por iteração: uma em heads e outra em tails.
  * Isso garante que haja chamadas equilibradas entre as opções.

* **my_messages.php**
  
  * Utilizar credenciais diferentes em CSV para simular múltiplos usuários.
  * Carregar com SharedArray no k6 para eficiência.
  * Cada VU usa credenciais distintas, evitando gargalo por repetição.


## Métricas Monitoradas

  * Latência (média, p95, p99).
  * Throughput (req/s).
  * Taxa de erro.
  * Checks de validação (status HTTP e conteúdo esperado).
  * Thresholds configurados no código:
    * http_req_duration: p(95) < 1000ms
    * http_req_failed: taxa < 5%


## Massa de Dados (`/my_messages.php`)

* Para simular logins realistas, criei um **projeto auxiliar em C#** chamado **GeradorDeUsuáriosAPP**, utilizando a biblioteca **Bogus**.
* Esse app gera **1000 usuários fictícios** com login/senha e grava em `usuarios.csv`.
* O arquivo é lido pelo k6 usando `SharedArray`, garantindo performance (o CSV só é carregado uma vez por worker).
* Cada usuário virtual (`VU`) utiliza credenciais diferentes, simulando acessos concorrentes ao endpoint e evitando gargalo por repetição de credenciais.

## Cenários de Carga

* **`/my_messages.php`**
  Utiliza configuração de **stages** (ramp-up progressivo e ramp-down):

  * 100 VUs em 30s
  * 500 VUs em 1 min
  * 1000 VUs em 1 min
  * Ramp-down em 30s

* **`/flip_coin.php`**
  Utiliza configuração de **scenarios** independentes:

  * 100 VUs por 30s
  * 500 VUs por 30s
  * 1000 VUs por 30s

Essa diferença mostra conhecimento das duas formas principais de configuração do k6: **stages** (carga progressiva) e **scenarios** (execuções paralelas/independentes).

## Relatórios em HTML

* Para atender ao item **2.2.3**, foi usada a biblioteca comunitária **[k6-reporter](https://github.com/benc-uk/k6-reporter)**.
* O `handleSummary` exporta os resultados em arquivos HTML:

  * `results/my_messages.html`
  * `results/flip_coin_test_result.html`
* Os relatórios apresentam métricas detalhadas de **latência (p95, p99)**, **throughput (req/s)**, **taxa de erro (http\_req\_failed)** e validações de **checks**.

## Boas Práticas Aplicadas

* Uso de **checks** para validar resposta HTTP e conteúdo da página, garantindo que não apenas haja resposta, mas que seja a esperada.
* Proteção dos checks (`r && r.body`) para evitar falhas de runtime e registrar erros corretamente no relatório.
* Separação de scripts por endpoint (`flip_coin_test.js` e `my_messages_test.js`), facilitando manutenção e reuso.
* Documentação clara da massa de dados e cenários de execução.

## ▶️ Como Executar os Testes

1. **Gerar massa de dados** (apenas necessário para o teste de `/my_messages.php`):

   * Compile e execute o projeto **GeradorDeUsuáriosAPP**
   * Isso criará o arquivo `usuarios.csv` em:

     ```
     ChallengeQa/TestData/usuarios.csv
     ```

2. **Executar os testes no k6**

   * Para `/my_messages.php`:

     ```bash
     k6 run my_messages_test.js
     ```
   * Para `/flip_coin.php`:

     ```bash
     k6 run flip_coin_test.js
     ```

3. **Resultados em HTML**

   * Após a execução, os relatórios estarão disponíveis em:

     ```
     results/my_messages.html
     results/flip_coin_test_result.html
     ```

4. **Análise dos resultados**

   * Abra os arquivos HTML no navegador para visualizar:

     * Distribuição de latência (p95, p99)
     * Throughput (req/s)
     * Taxa de erros (http\_req\_failed)
     * Checks validados durante o teste

