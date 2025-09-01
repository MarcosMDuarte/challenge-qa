using OpenQA.Selenium;
using ChallengeQa.Components;
using System.Collections.Generic;

namespace ChallengeQa.Pages
{
    public class CoursesPage
    {
        private readonly IWebDriver _driver;
        private readonly DropdownComponent _dropdownCursos;
        private readonly ButtonComponent _botaoAvancar;
        private readonly MessageComponent _mensagem;

        public CoursesPage(IWebDriver driver)
        {
            _driver = driver;

            _dropdownCursos = new DropdownComponent(
                driver,
                By.CssSelector("[data-testid='graduation-combo']"),
                By.XPath("//div[@role='option']")
            );

            _botaoAvancar = new ButtonComponent(driver, By.CssSelector("button[data-testid='next-button']"));
            _mensagem = new MessageComponent(driver, By.XPath("//h3"));
        }

        public void SelecionarCurso(string curso) => _dropdownCursos.Selecionar(curso);

        public List<string> ObterListaCursos() => _dropdownCursos.ObterOpcoes();

        public void Avancar() => _botaoAvancar.Clicar();

        public string ObterMensagem() => _mensagem.GetText();

        public string ObterMensagemAlerta()
        {
            var alerta = _driver.SwitchTo().Alert();
            var texto = alerta.Text;
            alerta.Accept();
            return texto;
        }

        public bool BotaoAvancarVisivel()
        {
            return _botaoAvancar.EstaVisivel();
        }
    }
}
