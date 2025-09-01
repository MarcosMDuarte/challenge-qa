using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using ChallengeQa.Pages;

namespace ChallengeQa.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private readonly LoginPage _loginPage;
        private readonly PersonalDataConfirmationPage _confirmationPage;
        private readonly RecoverUsernamePage _recoverUsernamePage;
        private readonly RecoverPasswordPage _recoverPasswordPage;
        private readonly ScenarioContext _scenarioContext;

        private string? _usuario;
        private string? _senha;

        public LoginSteps(LoginPage loginPage, PersonalDataConfirmationPage confirmationPage,
                          RecoverUsernamePage recoverUsernamePage, RecoverPasswordPage recoverPasswordPage, ScenarioContext scenarioContext)
        {
            _loginPage = loginPage;
            _confirmationPage = confirmationPage;
            _recoverUsernamePage = recoverUsernamePage;
            _recoverPasswordPage = recoverPasswordPage;
            _scenarioContext = scenarioContext;
        }

        // ---------------------------
        // Captura de credenciais
        // ---------------------------

        [When("capturo as credenciais geradas")]
        public void WhenCapturoAsCredenciaisGeradas()
        {
            (_usuario, _senha) = _confirmationPage.CapturarCredenciais();
            Console.WriteLine($"Credenciais capturadas: Usuario={_usuario}, Senha={_senha}");
        }

        // ---------------------------
        // Login principal
        // ---------------------------

        [When(@"acesso a área do candidato")]
        public void WhenAcessoAAreaDoCandidato() =>
            _confirmationPage.ClicarAcessarAreaCandidato();

        [When("preencho o usuário")]
        public void WhenPreenchoOUsuario()
        {
            var usuario = _scenarioContext.ContainsKey("usuario")
                ? _scenarioContext["usuario"].ToString()
                : throw new InvalidOperationException("Usuário não encontrado no contexto. Verifique se o cadastro foi concluído antes.");

            _loginPage.PreencherUsuario(usuario!);
        }

        [When(@"preencho o usuário e senha válidos")]
        public void WhenPreenchoOUsuarioESenhaValidos()
        {
            var usuario = _scenarioContext["usuario"].ToString();
            var senha = _scenarioContext["senha"].ToString();

            _loginPage.PreencherUsuario(usuario!);
            _loginPage.PreencherSenha(senha!);
        }

        [When(@"preencho usuário ""(.*)"" e senha ""(.*)""")]
        public void WhenPreenchoUsuarioESenha(string usuario, string senha)
        {
            _loginPage.PreencherUsuario(usuario);
            _loginPage.PreencherSenha(senha);
        }

        [When(@"clico em ""Login""")]
        public void WhenClicoEmLogin() =>
            _loginPage.ClicarLogin();

        [Then(@"devo visualizar a mensagem de erro (usuário|senha) ""(.*)""")]
        public void ThenDevoVisualizarAMensagemDeErroCampo(string campo, string mensagemEsperada)
        {
            var erro = campo.ToLower() switch
            {
                "usuário" => _loginPage.ObterErroUsuario(),
                "senha" => _loginPage.ObterErroSenha(),
                _ => throw new ArgumentException($"Campo inválido: {campo}")
            };

            Assert.That(erro, Does.Contain(mensagemEsperada).IgnoreCase);
            Console.WriteLine($"Erro validado no campo {campo}: {erro}");
        }

        [Then(@"devo visualizar a página inicial da área do candidato")]
        public void ThenDevoVisualizarAPaginaInicialDaAreaDoCandidato()
        {
            var mensagem = _loginPage.ObterMensagemInicial();

            Assert.That(mensagem, Does.Contain("Bem-vindo").IgnoreCase
                .Or.Contain("Área do candidato").IgnoreCase,
                $"Esperava mensagem de boas-vindas, mas veio: {mensagem}");

            Console.WriteLine("Página inicial validada com sucesso!");
        }



        // ---------------------------
        // Recuperar usuário
        // ---------------------------

        [When(@"clico em ""Recuperar usuário""")]
        public void WhenClicoEmRecuperarUsuario() =>
            _loginPage.ClicarRecuperarUsuario();

        [Then(@"devo visualizar a tela de recuperação de usuário")]
        public void ThenDevoVisualizarATelaDeRecuperacaoDeUsuario()
        {
            Assert.That(_recoverUsernamePage.ObterTitulo(), Does.Contain("usuário").IgnoreCase);
            Assert.That(_recoverUsernamePage.ObterMensagem(), Does.Match(".*email.*").IgnoreCase);
        }

        [Then(@"devo visualizar uma mensagem de erro solicitando o preenchimento do usuário")]
        public void ThenDevoVisualizarUmaMensagemDeErroSolicitandoOPreenchimentoDoUsuario()
        {
            var erro = _loginPage.ObterErroUsuario();
            Assert.That(erro, Is.Not.Empty, "Esperava mensagem de erro no campo usuário.");
        }

        // ---------------------------
        // Redefinir senha
        // ---------------------------

        [When(@"clico em ""Redefinir senha""")]
        public void WhenClicoEmRedefinirSenha() =>
            _loginPage.ClicarRedefinirSenha();

        [Then(@"devo visualizar a tela de redefinição de senha")]
        public void ThenDevoVisualizarATelaDeRedefinicaoDeSenha()
        {
            Assert.That(_recoverPasswordPage.ObterTitulo(), Does.Contain("senha").IgnoreCase);
            Assert.That(_recoverPasswordPage.ObterMensagem(), Does.Match(".*email.*").IgnoreCase);
        }

        [Then(@"devo visualizar uma mensagem de erro solicitando a identificação do usuário")]
        public void ThenDevoVisualizarUmaMensagemDeErroSolicitandoAIdentificacaoDoUsuario()
        {
            var erro = _loginPage.ObterErroUsuario();
            Assert.That(erro, Is.Not.Empty, "Esperava mensagem de erro solicitando identificação do usuário.");
        }

        // ---------------------------
        // Segurança
        // ---------------------------

        [Given(@"que acesso diretamente a URL da área do candidato sem estar autenticado")]
        public void GivenQueAcessoDiretamenteAUrlDaAreaDoCandidatoSemEstarAutenticado()
        {
            _loginPage.NavegarDireto("/subscription/candidate");
        }

        [Then(@"devo ser redirecionado para a tela de login")]
        public void ThenDevoSerRedirecionadoParaATelaDeLogin()
        {
            Assert.That(_loginPage.UrlAtual, Does.Contain("/subscription/authentication/login"));
        }
    }
}
