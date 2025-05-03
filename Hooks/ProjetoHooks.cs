using Newtonsoft.Json.Linq;
using RestSharp;
using TechTalk.SpecFlow;
using VotaE_BDD.Utils;

namespace VotaE_BDD.Hooks
{
    [Binding]
    public class ProjetoHooks
    {
        private readonly ScenarioContext _scenarioContext;

        public ProjetoHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario("projeto_valido")]
        public async Task CriarSugestaoValida()
        {
            var client = RestClientFactory.Create();
            var token = await TokenHelper.GetTokenAdminAsync();

            var request = new RestRequest("api/sugestao", Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddJsonBody(new
            {
                DadosHelper.Titulo,
                DadosHelper.Descricao,
                Localizacao = DadosHelper.LocalizacaoSugestao,
                Observacao = DadosHelper.ObservacaoSugestao,
                DadosHelper.DataCriacao,
                UsuarioId = 92,
                Usuario = new
                {
                    UsuarioId = 92,
                    Nome = "Ana Carolina",
                    Email = "anacarolina@teste.com",
                    Senha = "CmdX45",
                    Telefone = "456789",
                    UsuarioRole = "user"
                }
            });

            var response = await client.ExecuteAsync(request);
            var json = JObject.Parse(response.Content!);
            var sugestaoId = json["sugestaoId"]?.Value<int>() ?? throw new Exception("Falha ao obter sugestaoId");

            _scenarioContext["sugestaoId"] = sugestaoId;
            _scenarioContext["token"] = token;
        }
    }
}
