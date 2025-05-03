using Newtonsoft.Json.Linq;
using NJsonSchema;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;
using VotaE_BDD.Utils;

namespace VotaE_BDD.Steps.Projetos
{
    [Binding]
    public class CriarProjetosSteps
    {
        private RestClient _client;
        private RestResponse _response;
        private string _token = string.Empty;
        private readonly ScenarioContext _scenarioContext;

        public CriarProjetosSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"que estou autenticado com um token válido de admin")]
        public async Task GivenQueEstouAutenticadoComTokenValido()
        {
            _client = RestClientFactory.Create();
            _token = await TokenHelper.GetTokenAdminAsync();
        }

        //Primeiro cenário - Criar projeto - Status Code 200 e Validação JsonSchema
        [When(@"eu envio um projeto com dados fixos baseado em uma sugestão")]
        public async Task WhenEuEnvioProjeto()
        {
            _client = RestClientFactory.Create();

            var sugestaoId = _scenarioContext.Get<int>("sugestaoId");
            _token = _scenarioContext.Get<string>("token");

            var request = new RestRequest("api/projeto", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_token}");

            request.AddJsonBody(new
            {
                DadosHelper.Titulo,
                DadosHelper.Descricao,
                Status = "Aprovado",
                DataCadastro = DadosHelper.DataCriacao,
                DataEnvio = "2024-11-27T23:09:17.343Z",
                DataAprovacao = "2024-11-27T23:09:17.343Z",
                SugestaoId = sugestaoId
            });

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"o sistema deve retornar status 201 ao cadastrar projeto")]
        public void ThenDeveRetornarStatus201()
        {
            Assert.Equal(HttpStatusCode.Created, _response.StatusCode);
        }

        [Then(@"o projeto retornado deve conter os dados enviados")]
        public void ThenValidarTituloEStatus()
        {
            var json = JObject.Parse(_response.Content!);
            Assert.Equal(DadosHelper.Titulo, json["titulo"]?.ToString());
            Assert.Equal(DadosHelper.Descricao, json["descricao"]?.ToString());
            Assert.Equal("Aprovado", json["status"]?.ToString());
        }

        [Then(@"o projeto retornado deve conter um projetoId não nulo")]
        public void ThenProjetoRetornadoDeveConterProjetoId()
        {
            var json = JObject.Parse(_response.Content!);
            Assert.False(string.IsNullOrEmpty(json["projetoId"]?.ToString()), "O campo 'projetoId' está nulo ou vazio.");
        }

        // Validar Json Schema
        [Then(@"a resposta deve estar em conformidade com o schema de projeto")]
        public async Task ThenRespostaDeveEstarEmConformidadeComSchema()
        {
            var schemaPath = Path.Combine("Schemas", "projeto.schema.json");
            var schema = await JsonSchema.FromFileAsync(schemaPath);

            var errors = schema.Validate(_response.Content!);

            Assert.True(errors.Count == 0, "Contrato inválido: " + string.Join(" | ", errors.Select(e => e.ToString())));
        }

        // Segundo cenário - Bad Request ao enviar um projeto sem sugestaoId
        [When(@"eu envio um projeto sem informar o campo sugestaoId")]
        public async Task WhenEnvioProjetoSemSugestaoId()
        {
            var request = new RestRequest("api/projeto", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_token}");

            request.AddJsonBody(new
            {
                Titulo = "Parque para as crianças",
                Descricao = "Parque Infantil",
                Status = "Aprovado",
                DataCadastro = "2024-11-25T22:09:01",
                DataEnvio = "2024-11-27T23:09:17.343Z",
                DataAprovacao = "2024-11-27T23:09:17.343Z"
            });

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"o sistema deve retornar status 400 ao cadastrar projeto")]
        public void ThenDeveRetornarStatus400Projeto()
        {
            Assert.Equal(HttpStatusCode.BadRequest, _response.StatusCode);
        }

        //Terdceiro cenário - buscar um Id inexistente
        [When(@"eu buscar um projeto com Id (.*)")]
        public async Task WhenBuscarProjetoPorIdInexistente(int id)
        {
            var request = new RestRequest($"api/projeto/{id}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {_token}");
            request.AddHeader("Content-Type", "application/json");

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"o sistema deve retornar status 404 ao buscar projeto")]
        public void ThenDeveRetornar404ProjetoInvalido()
        {
            Assert.Equal(HttpStatusCode.NotFound, _response.StatusCode);
        }
    }
}
