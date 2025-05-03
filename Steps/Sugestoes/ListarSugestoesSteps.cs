using Newtonsoft.Json.Linq;
using NJsonSchema;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;
using VotaE_BDD.Utils;

namespace VotaE_BDD.Steps
{
    [Binding]
    public class ListarSugestoesSteps
    {
        private RestClient _client = null!;
        private RestResponse _response = null!;
        private string? _token;

        //Primeiro cenário - Status Code 200 ok - Validação Json Schema
        [Given(@"que estou autenticado com um token válido de usuário")]
        public async Task GivenQueEstouAutenticadoComoAdmin()
        {
            _client = RestClientFactory.Create();
            _token = await TokenHelper.GetTokenUserAsync(); 
        }

        [When(@"eu solicito a lista de sugestões")]
        public async Task WhenEuSolicitoListaSugestoes()
        {
            var request = new RestRequest("api/sugestao", Method.Get);
            request.AddHeader("Authorization", $"Bearer {_token}");
            request.AddHeader("Content-Type", "application/json");

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"o sistema deve retornar status 200 ao listar sugestões")]
        public void ThenStatus200()
        {
            Assert.Equal(HttpStatusCode.OK, _response.StatusCode);
        }

        [Then(@"a resposta deve estar em conformidade com o schema de sugestao")]
        public async Task ThenValidarContratoSchema()
        {
            var schemaPath = Path.Combine("Schemas", "sugestao-lista.schema.json");
            var schema = await JsonSchema.FromFileAsync(schemaPath);

            var array = JArray.Parse(_response.Content!);
            var errors = schema.Validate(array);

            Assert.True(errors.Count == 0, "Contrato inválido: " + string.Join(" | ", errors));
        }

        // Segundo Cenário - Status Code 401
        [Given(@"que não estou autenticado")]
        public void GivenQueNaoEstouAutenticado()
        {
            _client = RestClientFactory.Create();
            _token = null;
        }

        [Then(@"o sistema deve me retornar status 401 Unauthorized")]
        public void ThenStatus401()
        {
            Assert.Equal(HttpStatusCode.Unauthorized, _response.StatusCode);
        }
    }
}
