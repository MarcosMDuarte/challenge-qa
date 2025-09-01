using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

public static class WebDriverFactory
{
    public static bool IsLocalExecution { get; private set; }

    public static IWebDriver CreateDriver(string browser = "chrome")
    {
        IWebDriver driver;
        var gridUri = new Uri("http://localhost:4444/wd/hub");

        try
        {
            IsLocalExecution = false;
            switch (browser.ToLower())
            {
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    driver = new RemoteWebDriver(gridUri, firefoxOptions.ToCapabilities(), TimeSpan.FromSeconds(60));
                    break;

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    driver = new RemoteWebDriver(gridUri, edgeOptions.ToCapabilities(), TimeSpan.FromSeconds(60));
                    break;

                case "chrome":
                default:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--start-maximized");
                    chromeOptions.AddArgument("--disable-notifications");
                    chromeOptions.AddArgument("--disable-popup-blocking");
                    driver = new RemoteWebDriver(gridUri, chromeOptions.ToCapabilities(), TimeSpan.FromSeconds(60));
                    break;
            }
        }
        catch (WebDriverException)
        {
            IsLocalExecution = true;
            switch (browser.ToLower())
            {
                case "firefox":
                    driver = new FirefoxDriver();
                    break;

                case "edge":
                    driver = new EdgeDriver();
                    break;

                case "chrome":
                default:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--start-maximized");
                    chromeOptions.AddArgument("--disable-notifications");
                    chromeOptions.AddArgument("--disable-popup-blocking");
                    driver = new ChromeDriver(chromeOptions);
                    break;
            }
        }

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        return driver;
    }
}
