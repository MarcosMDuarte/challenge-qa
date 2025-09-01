# COMMENTS.md

## Resumo Executivo

Este projeto foi desenvolvido para atender ao desafio de automação de testes, aplicando **boas práticas de arquitetura**, **padrões de projeto** e **estratégias de testes funcionais e de performance**. A solução inclui componentização, Page Object Model, execução em paralelo e integração com Selenium Grid, além de massa de dados organizada em JSON. Embora alguns pontos ainda possam ser evoluídos (execução cross-browser completa e refinamento de relatórios de performance), o resultado cobre os requisitos principais e demonstra capacidade de estruturar testes de forma **escalável, organizada e sustentável**.

---

## Decisões de Arquitetura

### Componentização

Criei **Components reutilizáveis** (como `InputComponent`, `ButtonComponent`, `MessageComponent`) para encapsular interações com elementos da página, garantindo reuso e manutenção simplificada.

### Page Objects (POM)

Estrutura baseada em **Pages** (`SubscriptionPage`, `CoursesPage`, `PersonalDataPage`), que concentram a lógica de interação com a UI. Dessa forma, os **Steps** ficam limpos e mais legíveis.

### Steps Definition organizados

Os cenários foram ligados diretamente a métodos de `Steps` (`SubscriptionSteps`) sem duplicação de código, utilizando o utilitário **JsonUtils** para carregar dados dinâmicos. Esse utilitário encapsula leitura e desserialização com `Newtonsoft.Json`, centralizando o acesso à massa de dados.

### Driver Factory flexível

Implementei a **WebDriverFactory** com suporte a **Selenium Grid via Docker**. Em caso de falha na conexão com o Grid, o fallback executa localmente no navegador da máquina, facilitando o desenvolvimento e garantindo resiliência.

### Execução em paralelo

Ativei a execução paralela usando **NUnit**:

```csharp
[assembly: Parallelizable(ParallelScope.All)]
[assembly: LevelOfParallelism(3)]
```

Isso permite rodar cenários simultâneos e otimizar o tempo de execução.

---

## Bibliotecas de Terceiros

### Utilizadas

* **Reqnroll** → escrita dos cenários BDD em Gherkin.
* **NUnit** → execução dos testes e suporte a paralelismo.
* **Selenium WebDriver** → automação web.
* **Selenium Grid (Docker)** → execução distribuída e cross-browser.
* **Newtonsoft.Json** → leitura e desserialização de massas de dados JSON.

### Extras consideradas

* **Bogus** → geração de dados fake para cadastros.

---

## Estratégia de Testes

### Funcionais (E2E)

Cobri:

* Login.
* Navegação entre níveis/cursos.
* Cadastro com dados válidos e inválidos.
* Validações de mensagens de erro.
* Fluxo até a confirmação final.

### Validação de erros

Os testes que falham foram **mantidos** e devidamente documentados no `BUGS.md`, para rastreabilidade entre falhas e evidências. Exemplo: redefinição de senha sem usuário informado retorna mensagem incorreta.

### Dados de teste

Massa organizada em arquivos **JSON** (`cursos.json`, `cadastro.json`, `cadastro_mensagens.json`), centralizada pelo `JsonUtils`.

### Evidências

Screenshots e saídas foram salvos na pasta **Evidences**, organizados por cenário.

### Organização por Tags

Tag funcional → utilizada para agrupar cenários por funcionalidade (ex.: @ListaCursos, @Navegacao, @CadastroInvalido). Isso facilita a execução seletiva de testes (ex.: rodar apenas os cenários de cadastro) e também melhora a leitura e organização dos relatórios.

Tag de rastreabilidade → cada cenário recebe um ID único (ex.: @GT-001 … @GT-017). Esse identificador garante rastreabilidade ponta a ponta: é possível relacionar cenários com casos de teste documentados, issues reportadas no BUGS.md, evidências salvas e até mesmo pipelines de execução. Assim, se um teste falhar em produção, fica simples identificar exatamente qual cenário, qual requisito e qual bug estão envolvidos.

### Performance (Atividade 2)

* Foram criados scripts separados para `/flip_coin.php` e `/my_messages.php`, utilizando recursos de **stages** e **scenarios** do k6.
* Massa de dados gerada com app auxiliar em C# (Bogus) para 1000 usuários em `usuarios.csv`.
* Relatórios HTML exportados via **k6-reporter** (`results/my_messages.html`, `results/flip_coin_test_result.html`).
* Documentação detalhada disponível no arquivo **k6-data.md**, descrevendo objetivos, massa de dados, métricas monitoradas, cenários de carga e instruções de execução.

---

## Melhorias Futuras

* Integrar a massa de dados com **PostgreSQL**, substituindo arquivos estáticos.

---

## Conclusão

O projeto foi desenvolvido aplicando **boas práticas de automação de testes** (BDD, POM, componentização, paralelismo e execução distribuída), mantendo rastreabilidade entre cenários, dados e evidências. A solução cobre os requisitos principais e demonstra a capacidade de estruturar testes de forma **escalável e sustentável**.
