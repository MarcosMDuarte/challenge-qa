using OpenQA.Selenium;

namespace ChallengeQa.Components
{
    public class DateInputComponent
    {
        private readonly IWebDriver _driver;
        private readonly By _rootLocator;

        public DateInputComponent(IWebDriver driver, By rootLocator)
        {
            _driver = driver;
            _rootLocator = rootLocator;
        }

        /// <summary>
        /// Preenche a data no formato dd/MM/yyyy
        /// </summary>
        public void Preencher(string valor)
        {
            var partes = valor.Split('/');
            if (partes.Length != 3)
                throw new ArgumentException("Data deve estar no formato dd/MM/yyyy", nameof(valor));

            var dia = partes[0];
            var mes = partes[1];
            var ano = partes[2];

            var root = _driver.FindElement(_rootLocator);

            var diaInput = root.FindElement(By.CssSelector("[data-radix-vue-date-field-segment='day']"));
            var mesInput = root.FindElement(By.CssSelector("[data-radix-vue-date-field-segment='month']"));
            var anoInput = root.FindElement(By.CssSelector("[data-radix-vue-date-field-segment='year']"));

            PreencherCampo(diaInput, dia);
            PreencherCampo(mesInput, mes);
            PreencherCampo(anoInput, ano);
        }

        private void PreencherCampo(IWebElement input, string valor)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", input);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].focus();", input);

            // Apaga o valor existente (com backspaces)
            var existente = input.Text;
            for (int i = 0; i < existente.Length; i++)
                input.SendKeys(Keys.Backspace);

            // Digita só números
            foreach (char c in valor)
            {
                if (char.IsDigit(c))
                    input.SendKeys(c.ToString());
            }
        }


        /// <summary>
        /// Retorna a mensagem de erro associada ao campo de data
        /// </summary>
        public string ObterErro()
        {
            try
            {
                var erro = _driver.FindElement(By.XPath("//p[@role='alert']"));
                return erro.Text.Trim();
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }
        }
    }
}
