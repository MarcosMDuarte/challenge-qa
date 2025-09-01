using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.Linq;

namespace ChallengeQa.Components
{
    public class DropdownComponent
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly By _trigger;
        private readonly By _options;

        public DropdownComponent(IWebDriver driver, By trigger, By options)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            _trigger = trigger;
            _options = options;
        }

        public void Selecionar(string texto)
        {
            var combo = _wait.Until(d => d.FindElement(_trigger));
            combo.Click();
            var opcao = _wait.Until(d =>
                d.FindElement(By.XPath($"//div[@role='option' and normalize-space()='{texto}']"))
            );
            opcao.Click();
        }

        public List<string> ObterOpcoes()
        {
            var combo = _wait.Until(d => d.FindElement(_trigger));
            combo.Click();
            var elementos = _wait.Until(d => d.FindElements(_options));
            return elementos.Select(e => e.Text.Trim()).Where(t => t != "").ToList();
        }
    }
}
