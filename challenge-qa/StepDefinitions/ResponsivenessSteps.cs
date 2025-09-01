using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using ChallengeQa.Pages;
using System.Drawing;

namespace ChallengeQa.StepDefinitions
{
    [Binding]
    public class ResponsivenessSteps
    {
        private readonly IWebDriver _driver;
        private readonly CoursesPage _coursesPage;

        public ResponsivenessSteps(IWebDriver driver)
        {
            _driver = driver;
            _coursesPage = new CoursesPage(driver);
        }

        [When(@"defino a janela para ""(.*)""")]
        public void WhenDefinoAJanelaPara(string resolucao)
        {
            var parts = resolucao.Split('x');
            int largura = int.Parse(parts[0]);
            int altura = int.Parse(parts[1]);
            _driver.Manage().Window.Size = new Size(largura, altura);
        }

        [Then(@"o botão ""Avançar"" deve estar visível")]
        public void ThenOBotaoAvancarDeveEstarVisivel()
        {
            bool visivel = _coursesPage.BotaoAvancarVisivel();
            Assert.That(visivel, Is.True, "O botão 'Avançar' não está visível nessa resolução.");
        }
    }
}
