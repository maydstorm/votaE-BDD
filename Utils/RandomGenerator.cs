namespace VotaE_BDD.Utils
{
    public static class RandomGenerator
    {
        public static string GerarEmailUnico()
        {
            return $"usuario{Guid.NewGuid().ToString("N").Substring(0, 8)}@teste.com";
        }

        public static string GerarNome()
        {
            return "Usuário Teste " + new Random().Next(1000, 9999);
        }

        public static string GerarTelefone()
        {
            return "119" + new Random().Next(10000000, 99999999);
        }

        public static string GerarSenha()
        {
            var guid = Guid.NewGuid().ToString("N");
            var baseSenha = guid.Substring(0, 8);
            return $"S@{baseSenha}"; 
        }
    }
}
