using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ChallengeQa.Components
{
    public class CredentialComponent
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public CredentialComponent(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public (string Usuario, string Senha) Capturar()
        {
            var usuarioElem = _wait.Until(d => d.FindElement(By.XPath("(//p/strong)[1]")));
            var senhaElem = _wait.Until(d => d.FindElement(By.XPath("(//p/strong)[2]")));

            return (usuarioElem.Text.Trim(), senhaElem.Text.Trim());
        }
    }
}
