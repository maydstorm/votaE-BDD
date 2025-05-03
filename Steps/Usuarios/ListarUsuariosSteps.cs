using Newtonsoft.Json.Linq;
using NJsonSchema;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;
using VotaE_BDD.Utils;

namespace VotaE_BDD.Steps.Usuarios
{
    [Binding]
    public class ListarUsuariosSteps
    {
        private RestClient _client = null!;
        private RestResponse _response = null!;
        private string? _token;

        //Primeiro cenário - Status Code 200 - Validar JsonSchema
        [Given(@"que estou autenticado com um token de administrador")]
        public async Task GivenQueEstouAutenticadoComoAdmin()
        {
            _client = RestClientFactory.Create();
            _token = await TokenHelper.GetTokenAdminAsync(); 
        }

        [When(@"eu solicito a lista de usuários")]
        public async Task WhenEuSolicitoListaUsuarios()
        {
            var request = new RestRequest("api/usuario", Method.Get);
            request.AddHeader("Authorization", $"Bearer {_token}");
            request.AddHeader("Content-Type", "application/json");

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"o sistema deve retornar status 200 ao listar usuários")]
        public void ThenStatus200()
        {
            Assert.Equal(HttpStatusCode.OK, _response.StatusCode);
        }

        [Then(@"a resposta deve estar em conformidade com o schema de usuario")]
        public async Task ThenValidarSchemaDeUsuario()
        {
            var schemaPath = Path.Combine("Schemas", "usuario-lista.schema.json");
            var schema = await JsonSchema.FromFileAsync(schemaPath);

            var array = JArray.Parse(_response.Content!);
            var errors = schema.Validate(array);

            Assert.True(errors.Count == 0, "Contrato inválido: " + string.Join(" | ", errors));
        }

        //Segundo cenário - Status Code 403 para usuário comum
        [Given(@"que estou autenticado com um token de usuário comum")]
        public async Task GivenQueEstouAutenticadoComoUsuarioComum()
        {
            _client = RestClientFactory.Create();
            _token = await TokenHelper.GetTokenUserAsync();
        }

        [Then(@"o sistema deve me retornar status 403 Forbidden")]
        public void ThenStatus403()
        {
            Assert.Equal(HttpStatusCode.Forbidden, _response.StatusCode);
        }
    }
}
