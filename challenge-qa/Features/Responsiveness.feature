Feature: Testes de Responsividade
  Como usuário em diferentes dispositivos
  Quero visualizar corretamente o botão "Avançar"
  Para conseguir prosseguir no fluxo de inscrição


@Login @Responsividade
Scenario Outline: Fluxo de login completo em <resolucao>
  Given que acesso a página inicial com cadastro concluído no curso "<curso>" do nível "<nivel>"
  When defino a janela para "<resolucao>"
  And preencho o usuário e senha válidos
  And clico em "Login"
  Then devo visualizar a página inicial da área do candidato

Examples:
  | curso                           | nivel          | resolucao  |
  | Mestrado em Engenharia de Software | Pós-graduação | 1366x768   |
  | Direito                         | Graduação      | 1920x1080  |
  | Psicologia                      | Graduação      | 375x812    |


