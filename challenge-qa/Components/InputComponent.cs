using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ChallengeQa.Components
{
    public class InputComponent
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly By _selector;

        public InputComponent(IWebDriver driver, By selector)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            _selector = selector;
        }

        public void Preencher(string valor)
        {
            var campo = _wait.Until(d => d.FindElement(_selector));
            campo.Clear();
            campo.SendKeys(valor);
        }

        public string ObterErro()
        {
            var campo = _wait.Until(d => d.FindElement(_selector));
            try
            {
                var erro = campo.FindElement(By.XPath("./following-sibling::p[@role='alert']"));
                return string.IsNullOrWhiteSpace(erro.Text) ? "" : erro.Text.Trim();
            }
            catch (NoSuchElementException)
            {
                return "";
            }
        }
    }
}
