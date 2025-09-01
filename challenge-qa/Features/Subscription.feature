Feature: Subscription
  Como usuário
  Quero me inscrever em um curso
  Para acessar o formulário de matrícula

@ListaCursos @GT-001
Scenario Outline: Validação da lista de cursos
  Given que acesso a página de subscription
  When seleciono o nível "<nivel>"
  Then devo visualizar a lista de cursos de "<codigoNivel>"

Examples:
  | nivel          | codigoNivel   |
  | Graduação      | Graduacao     |
  | Pós-graduação  | PosGraduacao  |

@Navegacao @GT-002
Scenario Outline: Navegar até a página de <nivel> e retornar
  Given que acesso a página de subscription
  When seleciono o nível "<nivel>"
  Then devo visualizar a mensagem "Selecione seu curso de <nivel>"
  When clico no botão voltar
  Then devo visualizar a mensagem "Escolha o seu nível de ensino e embarque nessa aventura!"

Examples:
  | nivel         |
  | Graduação     |
  | Pós-graduação |


@FluxoCadastro @GT-003
Scenario Outline: Validação do fluxo de cadastro para cada curso
  Given que acesso a página de subscription
  When seleciono o nível "<nivel>"
  Then cada curso de "<codigoNivel>" deve permitir avançar para o formulário

Examples:
  | nivel          | codigoNivel   |
  | Graduação      | Graduacao     |
  | Pós-graduação  | PosGraduacao  |


@SelecaoCursoInvalida @GT-004
Scenario Outline: Tentativa de avançar sem selecionar um curso de <nivel>
  Given que acesso a página de subscription
  When seleciono o nível "<nivel>"
  And clico em Avançar sem selecionar um curso
  Then devo visualizar o alerta "Por favor, selecione um curso..."

Examples:
  | nivel         |
  | Graduação     |
  | Pós-graduação |

@CadastroCompleto @GT-005
Scenario: Cadastro com todos os campos preenchidos corretamente
  Given que acesso a página de subscription
  When seleciono o nível "Pós-graduação"
  And seleciono o curso "Mestrado em Engenharia de Software"
  And clico em "Avançar" no curso
  And preencho o formulário de cadastro com dados válidos
  And clico em "Avançar" no cadastro
  Then devo visualizar a mensagem "Sua jornada começa aqui!"


@CadastroInvalido @GT-006
Scenario Outline: Cadastro com <campo> inválido (<tipo>)
  Given que acesso a página de subscription
  When seleciono o nível "Pós-graduação"
  And seleciono o curso "Mestrado em Engenharia de Software"
  And clico em "Avançar" no curso
  And preencho o formulário de cadastro com <campo> inválido do tipo <tipo>
  And clico em "Avançar" no cadastro
  Then devo visualizar a mensagem de erro de <campo> do tipo <tipo> 

Examples:
  | campo     | tipo     |
  | CPF   | curto       |
  | CPF   | longo       |
  | CPF   | letras      |
  | CPF   | vazio       |
  | Nome      | vazio    |
  | Sobrenome | vazio    |
  | Email     | falta_arroba |
  | Email     | falta_tld   |
  | Email     | dois_pontos |
  | Email     | comeca_ponto |
  | Email     | tld_curto   |
  | Celular   | curto    |
  | Celular   | longo    |
  | Celular | letras  |
  | Celular | vazio   |
  | CEP   | curto       |
  | CEP   | longo       |
  | CEP   | letras      |
  | CEP   | caracteres  |
  | CEP   | vazio       |
  | Cidade    | vazio    |
  | Estado    | vazio    |
  | Pais      | vazio    |
  | Data      | futuro   |
  | Telefone   | curto    |
  | Telefone   | longo    |
  | Telefone   | letras  |


