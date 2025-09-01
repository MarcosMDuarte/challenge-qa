Feature: Política de Privacidade
  Como usuário
  Quero acessar a página de Política de Privacidade
  Para entender como meus dados são tratados

  @PrivacyPolicy @GT-014
  Scenario: Validar a página de Política de Privacidade no Chrome
    Given que acesso a página "https://developer.grupoa.education/subscription/" no chrome
    When clico no link "Política de Privacidade"
    Then devo visualizar o título "Política de Privacidade"

  @PrivacyPolicy @GT-015
  Scenario: Validar a página de Política de Privacidade no Firefox
    Given que acesso a página "https://developer.grupoa.education/subscription/" no firefox
    When clico no link "Política de Privacidade"
    Then devo visualizar o título "Política de Privacidade"

  @PrivacyPolicy @GT-016
  Scenario: Validar a página de Política de Privacidade no Edge
    Given que acesso a página "https://developer.grupoa.education/subscription/" no edge
    When clico no link "Política de Privacidade"
    Then devo visualizar o título "Política de Privacidade"
