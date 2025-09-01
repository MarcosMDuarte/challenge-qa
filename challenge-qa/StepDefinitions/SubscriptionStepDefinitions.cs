using NUnit.Framework;
using NUnit.Framework.Legacy;
using ChallengeQa.Pages;
using ChallengeQa.Utils;

namespace ChallengeQa.StepDefinitions
{
    [Binding]
    public class SubscriptionSteps
    {
        private readonly SubscriptionPage _subscriptionPage;
        private readonly CoursesPage _coursesPage;
        private readonly PersonalDataPage _personalDataPage;
        private readonly PersonalDataConfirmationPage _confirmationPage;

        public SubscriptionSteps(SubscriptionPage subscriptionPage, CoursesPage coursesPage,
                                 PersonalDataPage personalDataPage, PersonalDataConfirmationPage confirmationPage)
        {
            _subscriptionPage = subscriptionPage;
            _coursesPage = coursesPage;
            _personalDataPage = personalDataPage;
            _confirmationPage = confirmationPage;
        }

        // ---------------------------
        // Subscription Home
        // ---------------------------

        [Given(@"que acesso a página de subscription")]
        public void GivenQueAcessoAPaginaDeSubscription() =>
            _subscriptionPage.AcessarPagina();

        [Given(@"que acesso a página de subscription no ""(.*)""")]
        public void GivenQueAcessoAPaginaDeSubscriptionNo(string browser)
        {
            _subscriptionPage.AcessarPagina();
            Console.WriteLine($"Rodando no navegador: {browser}");
        }

        [When(@"seleciono o nível ""(.*)""")]
        public void WhenSelecionoONivel(string nivel) =>
            _subscriptionPage.SelecionarNivel(nivel);

        [Then(@"devo visualizar a mensagem ""(.*)""")]
        public void ThenDevoVisualizarAMensagem(string mensagemEsperada)
        {
            var mensagem = _subscriptionPage.ObterMensagemNivel();
            Assert.That(mensagem, Does.Contain(mensagemEsperada).IgnoreCase);
            Console.WriteLine("Mensagem validada: " + mensagem);
        }

        [When(@"clico no botão voltar")]
        public void WhenClicoNoBotaoVoltar() =>
            _subscriptionPage.ClicarVoltar();

        [Then(@"devo ver a mensagem inicial ""(.*)""")]
        public void ThenDevoVerAMensagemInicial(string mensagemEsperada)
        {
            var mensagem = _subscriptionPage.ObterMensagemInicial();
            Assert.That(mensagem, Does.Contain(mensagemEsperada).IgnoreCase);
            Console.WriteLine("Mensagem inicial validada: " + mensagem);
        }

        // ---------------------------
        // Courses
        // ---------------------------

        [Then(@"devo visualizar a lista de cursos de ""(.*)""")]
        public void ThenDeveExibirAListaDeCursosDe(string nivel)
        {
            var cursosEsperados = JsonUtils.CarregarCursos()[nivel];
            var cursosAtuais = _coursesPage.ObterListaCursos();

            CollectionAssert.AreEqual(cursosEsperados, cursosAtuais);
            Console.WriteLine($"Lista de cursos de {nivel} validada com sucesso!");
        }

        [When(@"seleciono o curso ""(.*)""")]
        public void WhenSelecionoOCurso(string curso) =>
            _coursesPage.SelecionarCurso(curso);

        [When(@"clico em ""Avançar"" no curso")]
        public void WhenClicoEmAvancarNoCurso() =>
            _coursesPage.Avancar();

        [Then(@"cada curso de ""(.*)"" deve permitir avançar para o formulário")]
        public void ThenCadaCursoDevePermitirAvancarParaOFormulario(string nivel)
        {
            var cursos = JsonUtils.CarregarCursos()[nivel];

            foreach (var curso in cursos)
            {
                _coursesPage.SelecionarCurso(curso);
                _coursesPage.Avancar();

                var mensagem = _personalDataPage.ObterMensagem();
                Assert.That(mensagem, Does.Contain("Pronto para essa aventura").IgnoreCase);

                Console.WriteLine($"Curso {curso} validado com sucesso!");

                _personalDataPage.Voltar();
            }
        }

        [When(@"clico em Avançar sem selecionar um curso")]
        public void WhenClicoEmAvancarSemSelecionarUmCurso()
        {
            _coursesPage.Avancar();
        }

        [Then(@"devo visualizar o alerta ""(.*)""")]
        public void ThenDevoVisualizarOAlerta(string mensagemEsperada)
        {
            var mensagemRecebida = _coursesPage.ObterMensagemAlerta();
            Assert.That(mensagemRecebida, Is.EqualTo(mensagemEsperada));
        }


        // ---------------------------
        // Personal Data (Cadastro)
        // ---------------------------

        [When(@"preencho o formulário de cadastro com dados válidos")]
        public void WhenPreenchoOFormularioDeCadastroComDadosValidos()
        {
            var dadosValidos = JsonUtils.CarregarCadastros()["cadastro_valido"];
            _personalDataPage.PreencherFormulario(dadosValidos);
        }

        [Then(@"devo ver a próxima etapa do cadastro")]
        public void ThenDevoVerAProximaEtapaDoCadastro()
        {
            var mensagem = _personalDataPage.ObterMensagem();
            Assert.That(mensagem, Does.Contain("Pronto para essa aventura").IgnoreCase);
            Console.WriteLine("Fluxo de cadastro validado com sucesso!");
        }

        [When(@"preencho o formulário de cadastro com (.*) inválido do tipo (.*)")]
        public void WhenPreenchoOFormularioDeCadastroComCampoInvalidoDoTipo(string campo, string tipo)
        {
            var cadastros = JsonUtils.CarregarCadastros();
            var dados = new Dictionary<string, string>(cadastros["cadastro_valido"]);

            var key = campo.ToLower();
            var chaveInvalida = $"{key}_{tipo.ToLower()}";

            if (!cadastros["cadastro_invalido"].ContainsKey(chaveInvalida))
                throw new KeyNotFoundException($"Não existe massa de dados inválida para {campo} do tipo {tipo}");

            dados[key] = cadastros["cadastro_invalido"][chaveInvalida];

            _personalDataPage.PreencherFormulario(dados);
        }


        [Then(@"devo visualizar a mensagem de erro de (.*) do tipo (.*)")]
        public void ThenDevoVisualizarMensagemDeErro(string campo, string tipo)
        {
            var mensagens = JsonUtils.CarregarMensagens(); // novo utilitário
            var chave = $"{campo.ToLower()}_{tipo.ToLower()}";

            if (!mensagens["mensagens_erro"].ContainsKey(chave))
                throw new KeyNotFoundException($"Mensagem de erro não configurada para {campo} do tipo {tipo}");

            var mensagemEsperada = mensagens["mensagens_erro"][chave];
            var mensagemRecebida = _personalDataPage.ObterErro(campo);

            Console.WriteLine($"[DEBUG] Campo: {campo} | Tipo: {tipo} | Esperado: '{mensagemEsperada}' | Recebido: '{mensagemRecebida}'");

            Assert.That(mensagemRecebida, Is.EqualTo(mensagemEsperada),
                $"Mensagem inesperada para {campo} ({tipo}). Recebido: '{mensagemRecebida}'");

            Console.WriteLine($"Erro validado no campo {campo} ({tipo}): {mensagemRecebida}");
        }




        [When(@"clico em ""Avançar"" no cadastro")]
        public void WhenClicoEmAvancarNoCadastro() =>
            _personalDataPage.Avancar();

        // ---------------------------
        // Personal Data Confirmation
        // ---------------------------

        [Then(@"devo ver a mensagem final ""(.*)""")]
        public void ThenDevoVerAMensagemFinal(string mensagemEsperada)
        {
            var mensagem = _confirmationPage.ObterMensagemFinal();
            Assert.That(mensagem, Does.Contain(mensagemEsperada).IgnoreCase);
            Console.WriteLine("Mensagem final validada: " + mensagem);
        }
    }
}
