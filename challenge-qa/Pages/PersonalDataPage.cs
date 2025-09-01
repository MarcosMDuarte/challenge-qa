using OpenQA.Selenium;
using ChallengeQa.Components;
using System.Collections.Generic;

namespace ChallengeQa.Pages
{
    public class PersonalDataPage
    {
        private readonly Dictionary<string, object> _inputs;
        private readonly ButtonComponent _botaoAvancar;
        private readonly MessageComponent _mensagem;
        private readonly ButtonComponent _botaoVoltar;

        public PersonalDataPage(IWebDriver driver)
        {
            _inputs = new()
            {
                {"cpf", new InputComponent(driver, By.CssSelector("[data-testid='cpf-input']"))},
                {"nome", new InputComponent(driver, By.CssSelector("[data-testid='name-input']"))},
                {"sobrenome", new InputComponent(driver, By.CssSelector("[data-testid='surname-input']"))},
                {"nome_social", new InputComponent(driver, By.CssSelector("[data-testid='social-name-input']"))},
                {"email", new InputComponent(driver, By.CssSelector("[data-testid='email-input']"))},
                {"celular", new InputComponent(driver, By.CssSelector("[data-testid='cellphone-input']"))},
                {"telefone", new InputComponent(driver, By.CssSelector("[data-testid='phone-input']"))},
                {"cep", new InputComponent(driver, By.CssSelector("[data-testid='cep-input']"))},
                {"endereco", new InputComponent(driver, By.CssSelector("[data-testid='address-input']"))},
                {"complemento", new InputComponent(driver, By.CssSelector("[data-testid='complement-input']"))},
                {"bairro", new InputComponent(driver, By.CssSelector("[data-testid='neighborhood-input']"))},
                {"cidade", new InputComponent(driver, By.CssSelector("[data-testid='city-input']"))},
                {"estado", new InputComponent(driver, By.CssSelector("[data-testid='state-input']"))},
                {"pais", new InputComponent(driver, By.CssSelector("[data-testid='country-input']"))},
                {"data", new DateInputComponent(driver, By.CssSelector("[data-testid='birthdate-input']"))}
            };

            _botaoAvancar = new ButtonComponent(driver, By.CssSelector("button[data-testid='next-button']"));
            _botaoVoltar = new ButtonComponent(driver, By.CssSelector("button[data-testid='back-button']"));
            _mensagem = new MessageComponent(driver, By.XPath("//h3"));
        }

        public void PreencherFormulario(Dictionary<string, string> dados)
        {
            foreach (var campo in dados)
            {
                Console.WriteLine($"Campo: {campo.Key} => Valor: {campo.Value}");
                var key = campo.Key.ToLower();

                if (_inputs.ContainsKey(key))
                {
                    if (_inputs[key] is InputComponent input)
                        input.Preencher(campo.Value);

                    else if (_inputs[key] is DateInputComponent dateInput)
                        dateInput.Preencher(campo.Value);
                }
            }
        }

        public void Avancar() => _botaoAvancar.Clicar();
        public void Voltar() => _botaoVoltar.Clicar();
        public string ObterMensagem() => _mensagem.GetText();

        public string ObterErro(string campo)
        {
            var key = campo.ToLower();

            if (_inputs[key] is InputComponent input)
                return input.ObterErro();

            if (_inputs[key] is DateInputComponent dateInput)
                return dateInput.ObterErro();

            return string.Empty;
        }
    }
}
