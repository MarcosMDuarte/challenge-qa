Feature: Login do candidato
  Como candidato
  Quero acessar a área do candidato
  Para visualizar minhas informações após o cadastro

Background:
  Given que concluo o cadastro de um candidato no curso "Mestrado em Engenharia de Software" do nível "Pós-graduação" recebendo credenciais válidas

    @Login @GT-007
    Scenario: Login com credenciais geradas no cadastro
        When acesso a área do candidato
        And preencho o usuário e senha válidos
        And clico em "Login"
        Then devo visualizar a página inicial da área do candidato

    @LoginInvalido @GT-008
    Scenario Outline: Tentativa de login inválido
        When acesso a área do candidato
        And preencho usuário "<usuario>" e senha "<senha>"
        And clico em "Login"
        Then devo visualizar a mensagem de erro <campo> "<mensagem>"

    Examples:
        | usuario         | senha             | campo    | mensagem          |
        | usuarioInvalido | subscription      | usuário  | Usuário inválido  |
        | candidato       | Senha_inválida    | senha    | Senha inválida    |
        | usuarioInvalido | Senha_inválida    | usuário  | Usuário inválido  |
        | usuarioInvalido | Senha_inválida    | senha    | Senha inválida    |


    @RecuperarUsuario @GT-009
    Scenario: Fluxo de recuperação de usuário
        When acesso a área do candidato
        And preencho o usuário
        And clico em "Recuperar usuário"
        Then devo visualizar a tela de recuperação de usuário

    @RecuperarUsuario @GT-010
    Scenario: Recuperação de usuário sem informar credenciais
        When acesso a área do candidato
        And clico em "Recuperar usuário"
        Then devo visualizar uma mensagem de erro solicitando o preenchimento do usuário


    @RedefinirSenha @GT-011
    Scenario: Fluxo de redefinição de senha
      When acesso a área do candidato
      And preencho o usuário
      And clico em "Redefinir senha"
      Then devo visualizar a tela de redefinição de senha

    @RedefinirSenha @GT-012
    Scenario: Redefinição de senha sem informar credenciais
      When acesso a área do candidato
      And clico em "Redefinir senha"
      Then devo visualizar uma mensagem de erro solicitando a identificação do usuário



      @Seguranca @GT-013
      Scenario: Tentativa de acesso direto sem login
        Given que acesso diretamente a URL da área do candidato sem estar autenticado
        Then devo ser redirecionado para a tela de login

