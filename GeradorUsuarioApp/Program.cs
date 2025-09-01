using Bogus;
using System.Text;

class Program
{
    static void Main()
    {
        int qtd = 1000;

        string pasta = @"C:\Desafio+A\challenge-qa\challenge-qa\TestData";

        if (!Directory.Exists(pasta))
        {
            Console.WriteLine($"[INFO] Criando pasta: {pasta}");
            Directory.CreateDirectory(pasta);
        }

        string arquivo = Path.Combine(pasta, "usuarios.csv");

        // 🔹 Escreve com UTF-8 para manter acentuação correta
        using (var writer = new StreamWriter(arquivo, false, Encoding.UTF8))
        {
            writer.WriteLine("login,password");

            var faker = new Faker("pt_BR");

            for (int i = 0; i < qtd; i++)
            {
                string nome = faker.Name.FirstName().ToLower();
                string sobrenome = faker.Name.LastName().ToLower();
                string login = $"{nome}.{sobrenome}{i}";
                string senha = $"Senha{i:0000}";

                writer.WriteLine($"{login},{senha}");
            }
        }

        Console.WriteLine($"✅ Arquivo gerado em: {arquivo}");
    }
}
