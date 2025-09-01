using Reqnroll;
using ChallengeQa.Pages;
using NUnit.Framework;

namespace ChallengeQa.StepDefinitions
{
    [Binding]
    public class SetupSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly SubscriptionPage _subscriptionPage;
        private readonly CoursesPage _coursesPage;
        private readonly PersonalDataPage _personalDataPage;
        private readonly PersonalDataConfirmationPage _confirmationPage;

        public SetupSteps(
            ScenarioContext scenarioContext,
            SubscriptionPage subscriptionPage,
            CoursesPage coursesPage,
            PersonalDataPage personalDataPage,
            PersonalDataConfirmationPage confirmationPage)
        {
            _scenarioContext = scenarioContext;
            _subscriptionPage = subscriptionPage;
            _coursesPage = coursesPage;
            _personalDataPage = personalDataPage;
            _confirmationPage = confirmationPage;
        }

        [Given(@"que concluo o cadastro de um candidato no curso ""(.*)"" do nível ""(.*)"" recebendo credenciais válidas")]
        public void GivenQueConcluoOCadastroDeUmCandidatoNoCursoDoNivel(string curso, string nivel)
        {
            // Fluxo de cadastro completo
            _subscriptionPage.AcessarPagina();
            _subscriptionPage.SelecionarNivel(nivel);
            _coursesPage.SelecionarCurso(curso);
            _coursesPage.Avancar();

            var dadosValidos = Utils.JsonUtils.CarregarCadastros()["cadastro_valido"];
            _personalDataPage.PreencherFormulario(dadosValidos);
            _personalDataPage.Avancar();

            // Captura credenciais
            var (usuario, senha) = _confirmationPage.CapturarCredenciais();

            // Armazena no contexto
            _scenarioContext["usuario"] = usuario;
            _scenarioContext["senha"] = senha;

            Console.WriteLine($"[Setup] Credenciais criadas: Usuario={usuario}, Senha={senha}");
        }

        [Given(@"que acesso a página inicial com cadastro concluído no curso ""(.*)"" do nível ""(.*)""")]
        public void GivenQueAcessoAPaginaInicialComCadastroConcluido(string curso, string nivel)
        {
            // Reaproveita o fluxo de cadastro parametrizado
            GivenQueConcluoOCadastroDeUmCandidatoNoCursoDoNivel(curso, nivel);

            // E já acessa a área do candidato
            _confirmationPage.ClicarAcessarAreaCandidato();
        }
    }
}
