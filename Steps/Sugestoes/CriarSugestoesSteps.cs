using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;
using VotaE_BDD.Utils;

namespace VotaE_BDD.Steps.Sugestoes
{
    [Binding]
    public class CriarSugestoesSteps
    {
        private RestClient _client;
        private RestResponse _response;
        private string _token = string.Empty;

        [Given(@"que estou autenticado com um token válido")]
        public async Task GivenQueEstouAutenticadoComTokenValido()
        {
            _client = RestClientFactory.Create();
            _token = await TokenHelper.GetTokenUserAsync();
        }

        //Primeiro Cenário - Enviar corretamente uma sugestão - Status Code 201 
        [When(@"eu envio uma sugestão com dados fixos")]
        public async Task WhenEuEnvioUmaSugestao()
        {
            var request = new RestRequest("api/sugestao", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_token}");

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

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"o sistema deve retornar status 201 Created ao criar uma sugestão")]
        public void ThenRetornoStatusCreated()
        {
            Assert.Equal(HttpStatusCode.Created, _response.StatusCode);
        }

        [Then(@"a sugestão retornada deve conter os dados enviados")]
        public void ThenSugestaoDeveConterTituloELocalizacao()
        {
            var json = JObject.Parse(_response.Content!);
            Assert.Equal(DadosHelper.Titulo, json["titulo"]?.ToString());
            Assert.Equal(DadosHelper.Descricao, json["descricao"]?.ToString());
            Assert.Equal(DadosHelper.LocalizacaoSugestao, json["localizacao"]?.ToString());
        }

        // Segundo cenário - Ao enviar uma sugestão sem usuário, deve retornar status code 400
        [When(@"eu envio uma sugestão sem informar o campo Usuario")]
        public async Task WhenEnvioSugestaoSemUsuario()
        {
            var request = new RestRequest("api/sugestao", Method.Post);
            request.AddHeader("Authorization", $"Bearer {_token}");

            request.AddJsonBody(new
            {
                Titulo = "Teste",
                Descricao = "Teste de autenticacao",
                Localizacao = "Avenida Tiquatira",
                Observacao = "Essa avenida tem um espaço que seria muito bom para construir um parque para as crianças terem um espaço de diversão.",
                DataCriacao = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"),
                UsuarioId = 92
            });

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"o sistema deve retornar status 400 ao validar sugestão")]
        public void ThenDeveRetornar400ValidacaoSugestao()
        {
            Assert.Equal(HttpStatusCode.BadRequest, _response.StatusCode);
        }
    }
}
