using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;


namespace VotaE_BDD.Utils
{
    public static class TokenHelper
    {
        private static IConfiguration config = new ConfigurationBuilder()
             .AddJsonFile("Properties/secrets.json", optional: false)
             .Build();

        public static async Task<string> GetTokenAdminAsync()
        {
            var client = RestClientFactory.Create();
            var request = new RestRequest("api/autenticacao/login", Method.Post); 

            request.AddJsonBody(new
            {
                email = config["Credenciais:Admin:Email"],
                senha = config["Credenciais:Admin:Senha"]
            });

            var response = await client.ExecuteAsync(request);

            var json = JObject.Parse(response.Content!);
            return json["token"]?.ToString() ?? throw new Exception("Token não encontrado");
        }

        public static async Task<string> GetTokenUserAsync()
        {
            var client = RestClientFactory.Create();
            var request = new RestRequest("api/autenticacao/login", Method.Post);

            request.AddJsonBody(new
            {
                email = config["Credenciais:Usuario:Email"],
                senha = config["Credenciais:Usuario:Senha"]
            });

            var response = await client.ExecuteAsync(request);

            var json = JObject.Parse(response.Content!);
            return json["token"]?.ToString() ?? throw new Exception("Token não encontrado");
        }
    }
}
