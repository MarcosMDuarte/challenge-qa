# DATA.md

## Estratégia de Persistência e Uso de Massa de Dados de Testes

### 1. Objetivo
Com a evolução da aplicação para persistir dados em **PostgreSQL**, a massa de dados de teste precisa ser migrada de arquivos estáticos (JSON/CSV) para tabelas no banco.  
Nesta estratégia, centralizamos os dados em uma única tabela `cadastros_teste`, que contempla tanto os **dados válidos** quanto os **inválidos**, incluindo as **regras de validação** e as **mensagens esperadas**.  
Assim, eliminamos a necessidade de uma tabela separada para mensagens de erro.

---

### 2. Estrutura de Dados

#### 2.1 Tabela `usuarios`
- **Origem**: `usuarios.csv`
- **Colunas**:
  - `id` (PK, SERIAL)
  - `login` (VARCHAR)
  - `password` (VARCHAR)
  - `perfil` (VARCHAR, opcional)
- **Finalidade**: simular diferentes perfis de acesso para login, autorização e testes de carga.

#### 2.2 Tabela `cursos`
- **Origem**: `cursos.json`
- **Colunas**:
  - `id` (PK, SERIAL)
  - `nome` (VARCHAR)
  - `nivel` (VARCHAR → ex.: Graduação, Pós-Graduação)
- **Finalidade**: disponibilizar lista de cursos para cenários de inscrição.

#### 2.3 Tabela `cadastros_teste`
- **Origem**: unificação de `cadastro.json` + `cadastro_mensagens.json`
- **Colunas**:
  - `id` (PK, SERIAL)
  - `cpf` (VARCHAR)
  - `nome` (VARCHAR)
  - `sobrenome` (VARCHAR)
  - `email` (VARCHAR)
  - `celular` (VARCHAR)
  - `telefone` (VARCHAR, opcional)
  - `cep` (VARCHAR)
  - `endereco` (VARCHAR)
  - `bairro` (VARCHAR)
  - `cidade` (VARCHAR)
  - `estado` (VARCHAR)
  - `pais` (VARCHAR)
  - `data_nascimento` (DATE)
  - `tipo_teste` (VARCHAR → `valido` | `invalido`)
  - `regra` (VARCHAR → ex.: `cpf_curto`, `email_falta_arroba`)
  - `mensagem_esperada` (VARCHAR → ex.: `CPF inválido`, `Email inválido`)
- **Observação**: esta tabela cobre tanto massa positiva quanto negativa, permitindo que cada registro já traga o input + expectativa de saída.

---

### 3. Estratégia de Criação

1. Criar diretório `/db/seeds/` no repositório contendo scripts:
   - `schema.sql` → criação das tabelas (`usuarios`, `cursos`, `cadastros_teste`).
   - `data.sql` → inserção de registros iniciais (válidos e inválidos).
2. Utilizar **chaves primárias automáticas (`SERIAL`/`BIGSERIAL`)**.
3. Inserir dados controlados por **scripts de seed**, permitindo reset da base para estado inicial antes da execução de testes.

---

### 4. Estratégia de Uso nos Testes

#### 4.1 Ambiente de Testes
- Banco dedicado (ex.: `qa_test_db`).
- Reset completo da base antes da suíte → consistência garantida.

#### 4.2 Carga Inicial
- Executar `schema.sql` + `data.sql`.
- Automatizar execução via CI/CD (Docker Compose, pipelines de teste).

#### 4.3 Testes Automatizados
- **E2E (Selenium/Reqnroll)** → utilizam cadastros válidos e inválidos do banco.
- **Carga (K6)** → reutilizam massa de usuários diretamente do PostgreSQL.
- **Validação de mensagens** → consulta campo `mensagem_esperada` da tabela `cadastros_teste`.

#### 4.4 Regeneração de Massa
- Utilizar **libraries de geração dinâmica** (Faker, Bogus, Factory Boy).
- Inserção automática de dados antes de cenários específicos.
- Possibilidade de truncar tabelas e repovoar a cada execução.

---

### 5. Benefícios

- **Centralização** da massa de dados no banco.  
- **Traçabilidade direta**: cada registro contém input + regra + mensagem esperada.  
- **Facilidade para testes automatizados**: basta selecionar registros por `tipo_teste` ou `regra`.  
- **Escalabilidade**: fácil expansão da massa para novos cenários.  
- **Reset simples**: scripts permitem restaurar estado inicial rapidamente.  
- **Versionamento**: scripts SQL versionados junto ao código-fonte.  

