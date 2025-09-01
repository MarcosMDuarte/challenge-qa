using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ChallengeQa.Components
{
    public class ButtonComponent
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly By _selector;

        public ButtonComponent(IWebDriver driver, By selector)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            _selector = selector;
        }

        public void Clicar()
        {
            var botao = _wait.Until(d => d.FindElement(_selector));
            botao.Click();
        }

        public bool EstaVisivel()
        {
            try
            {
                var botao = _wait.Until(d => d.FindElement(_selector));
                return botao.Displayed && botao.Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

    }
}
