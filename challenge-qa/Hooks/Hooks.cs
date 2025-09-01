using OpenQA.Selenium;
using Reqnroll.BoDi;

namespace ChallengeQa.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver? _driver;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            string browser = "chrome";

            if (scenarioContext.ScenarioInfo.Arguments.Contains("browser"))
            {
                var browserValue = scenarioContext.ScenarioInfo.Arguments["browser"];
                browser = browserValue?.ToString() ?? "chrome";
            }

            _driver = WebDriverFactory.CreateDriver(browser);
            _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
        }


        /*[AfterScenario]
        public void AfterScenario()
        {
            _driver?.Quit();
        }
        */
    }
}
