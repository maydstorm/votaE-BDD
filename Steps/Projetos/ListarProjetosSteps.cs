using Newtonsoft.Json.Linq;
using NJsonSchema;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;
using VotaE_BDD.Utils;

namespace VotaE_BDD.Steps.Projetos
{
    [Binding]
    public class ListarProjetosSteps
    {
        private RestClient _client = null!;
        private RestResponse _response = null!;
        private string? _token;

        // Primeiro Cenário - Status Code 200 e validar Json Schema
        [Given(@"que estou autenticado")]
        public async Task GivenAutenticado()
        {
            _client = RestClientFactory.Create();
            _token = await TokenHelper.GetTokenAdminAsync(); 
        }

        [When(@"eu solicito a lista de projetos")]
        public async Task WhenSolicitoListaProjetos()
        {
            var request = new RestRequest("api/projeto", Method.Get);
            request.AddHeader("Authorization", $"Bearer {_token}");
            request.AddHeader("Content-Type", "application/json");

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"o sistema deve retornar status 200 ao listar projetos")]
        public void ThenRetorno200()
        {
            Assert.Equal(HttpStatusCode.OK, _response.StatusCode);
        }

        [Then(@"a resposta deve estar em conformidade com o schema de listagem de projetos")]
        public async Task ThenValidarContrato()
        {
            var schemaPath = Path.Combine("Schemas", "projeto-lista.schema.json");
            var schema = await JsonSchema.FromFileAsync(schemaPath);

            var json = JArray.Parse(_response.Content!);
            var errors = schema.Validate(json);

            Assert.True(errors.Count == 0, "Contrato inválido: " + string.Join(" | ", errors));
        }

        // Segundo Cenário - Status Code 401 
        [Given(@"que não estou autenticado com um token")]
        public void GivenNaoAutenticado()
        {
            _client = RestClientFactory.Create();
            _token = null;
        }

        [Then(@"o sistema deve retornar status 401 Unauthorized")]
        public void ThenRetorno401()
        {
            Assert.Equal(HttpStatusCode.Unauthorized, _response.StatusCode);
        }
    }
}
