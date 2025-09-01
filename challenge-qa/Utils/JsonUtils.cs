using Newtonsoft.Json;

namespace ChallengeQa.Utils
{
    public static class JsonUtils
    {
        public static Dictionary<string, List<string>> CarregarCursos()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "cursos.json");
            var json = File.ReadAllText(path);
            var cursos = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
            if (cursos == null)
                throw new InvalidOperationException("Erro ao carregar o arquivo cursos.json");

            return cursos;
        }

        public static Dictionary<string, Dictionary<string, string>> CarregarCadastros()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "cadastro.json");
            var json = File.ReadAllText(path);
            var cadastros = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            if (cadastros == null)
                throw new InvalidOperationException("Erro ao carregar o arquivo cadastro.json");

            return cadastros;
        }

        public static Dictionary<string, Dictionary<string, string>> CarregarMensagens()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "cadastro_mensagens.json");
            var json = File.ReadAllText(path);
            var mensagens = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            if (mensagens == null)
                throw new InvalidOperationException("Erro ao carregar o arquivo cadastro_mensagens.json");

            return mensagens;
        }
    }
}
