using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ChallengeQa.Components;

namespace ChallengeQa.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly InputComponent _usuario;
        private readonly InputComponent _senha;
        private readonly ButtonComponent _botaoLogin;
        private readonly By _mensagemInicialLocator = By.XPath("//h1 | //h2 | //h3");


        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _usuario = new InputComponent(driver, By.CssSelector("[data-testid='username-input']"));
            _senha = new InputComponent(driver, By.CssSelector("[data-testid='password-input']"));
            _botaoLogin = new ButtonComponent(driver, By.CssSelector("button[data-testid='login-button']"));
        }

        public void PreencherUsuario(string usuario) => _usuario.Preencher(usuario);
        public void PreencherSenha(string senha) => _senha.Preencher(senha);
        public void ClicarLogin() => _botaoLogin.Clicar();

        public string ObterMensagemInicial()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            wait.Until(d => !d.Url.Contains("/authentication"));

            var mensagem = new MessageComponent(_driver, _mensagemInicialLocator);
            return mensagem.GetText();
        }


        public string ObterErroUsuario() => _usuario.ObterErro();
        public string ObterErroSenha() => _senha.ObterErro();

        public void ClicarRecuperarUsuario() =>
            _driver.FindElement(By.LinkText("Recuperar usuário")).Click();

        public void ClicarRedefinirSenha() =>
            _driver.FindElement(By.LinkText("Redefinir senha")).Click();

        public void NavegarDireto(string relativeUrl) =>
            _driver.Navigate().GoToUrl("https://developer.grupoa.education" + relativeUrl);

        public string UrlAtual => _driver.Url;
    }
}
