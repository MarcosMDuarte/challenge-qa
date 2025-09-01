using NUnit.Framework;
using OpenQA.Selenium;

[Binding]
public class PrivacySteps
{
    private readonly IWebDriver _driver;

    public PrivacySteps(IWebDriver driver)
    {
        _driver = driver;
    }

    [Given(@"que acesso a página ""(.*)"" no (.*)")]
    public void GivenQueAcessoAPaginaNoBrowser(string url, string browser)
    {
        _driver.Navigate().GoToUrl(url);
    }

    [When(@"clico no link ""(.*)""")]
    public void WhenClicoNoLink(string linkText)
    {
        var link = _driver.FindElement(By.CssSelector("[data-testid='nav-1-link']"));
        link.Click();
    }

    [Then(@"devo visualizar o título ""(.*)""")]
    public void ThenDevoVisualizarOTitulo(string expectedTitle)
    {
        var heading = _driver.FindElement(By.TagName("h1"));
        Assert.That(heading.Text, Does.Contain(expectedTitle));
    }
}
