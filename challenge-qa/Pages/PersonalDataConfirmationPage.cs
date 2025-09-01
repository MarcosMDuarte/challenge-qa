using OpenQA.Selenium;
using ChallengeQa.Components;

namespace ChallengeQa.Pages
{
    public class PersonalDataConfirmationPage
    {
        private readonly CredentialComponent _credenciais;
        private readonly MessageComponent _mensagemFinal;
        private readonly ButtonComponent _botaoAcessar;

        public PersonalDataConfirmationPage(IWebDriver driver)
        {
            _credenciais = new CredentialComponent(driver);
            _mensagemFinal = new MessageComponent(driver, By.XPath("//h2"));
            _botaoAcessar = new ButtonComponent(driver, By.CssSelector("button[data-testid='next-button']"));
        }

        public (string Usuario, string Senha) CapturarCredenciais() => _credenciais.Capturar();
        public string ObterMensagemFinal() => _mensagemFinal.GetText();
        public void ClicarAcessarAreaCandidato() => _botaoAcessar.Clicar();
    }
}
