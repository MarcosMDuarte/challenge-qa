using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ChallengeQa.Components
{
    public class MessageComponent
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly By _selector;

        public MessageComponent(IWebDriver driver, By selector)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            _selector = selector;
        }

        public string GetText()
        {
            for (int i = 0; i < 3; i++) 
            {
                try
                {
                    var elemento = _wait.Until(d => d.FindElement(_selector));
                    return elemento.Text.Trim();
                }
                catch (StaleElementReferenceException)
                {
                    Thread.Sleep(200); 
                }
            }

            throw new Exception($"Não foi possível capturar o texto do elemento localizado por {_selector}");
        }
    }
}
