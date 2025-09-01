using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ChallengeQa.Components;

namespace ChallengeQa.Pages
{
    public class SubscriptionPage
    {
        private readonly DropdownComponent _nivel;
        private readonly ButtonComponent _voltar;
        private readonly MessageComponent _mensagemNivel;
        private readonly MessageComponent _mensagemInicial;
        private readonly IWebDriver _driver;

        public SubscriptionPage(IWebDriver driver)
        {
            _driver = driver;
            _nivel = new DropdownComponent(driver,
                By.CssSelector("button[data-testid='education-level-select']"),
                By.XPath("//div[@role='option']")
            );
            _voltar = new ButtonComponent(driver, By.CssSelector("[data-testid='back-button']"));
            _mensagemNivel = new MessageComponent(driver, By.XPath("//*[@id='app']/main/section/div/div[1]/h3"));
            _mensagemInicial = new MessageComponent(driver, By.XPath("//h3"));
        }

        public void AcessarPagina() => _driver.Navigate().GoToUrl("https://developer.grupoa.education/subscription/");
        public void VoltarPagina()
        {
            _driver.Navigate().Back();
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.CssSelector("[data-testid='education-level-select']")));
        }
        public void SelecionarNivel(string nivel) => _nivel.Selecionar(nivel);
        public string ObterMensagemNivel() => _mensagemNivel.GetText();
        public void ClicarVoltar() => _voltar.Clicar();
        public string ObterMensagemInicial() => _mensagemInicial.GetText();
    }
}
