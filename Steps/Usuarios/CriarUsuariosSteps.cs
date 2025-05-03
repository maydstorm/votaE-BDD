using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;
using VotaE_BDD.Utils;

namespace VotaE_BDD.Steps.Usuarios
{
    [Binding]
    public class UsuarioSteps
    {
        private RestClient _client;
        private RestResponse _response;
        private string? _nome;
        private string? _email;
        private string? _telefone;
        private string? _senha;
        private readonly string _role = "user";
        private string? _emailTestado;

        
        [Given(@"que desejo criar um novo usuário com dados válidos e únicos")]
        public async Task GivenQueNaoExisteOEmailCadastrado()
        {
            _client = Utils.RestClientFactory.Create();
            _nome = RandomGenerator.GerarNome();
            _email = RandomGenerator.GerarEmailUnico();
            _telefone = RandomGenerator.GerarTelefone();
            _senha = RandomGenerator.GerarSenha();
        }

        //Primeiro cenário - Criar usuário
        [When(@"eu envio os dados para a API de cadastro")]
        public async Task WhenEnvioDadosValidos()
        {
            var request = new RestRequest("api/usuario", Method.Post);
            request.AddJsonBody(new
            {
                Nome = _nome,
                Email = _email,
                Senha = _senha,
                Telefone = _telefone,
                UsuarioRole = _role
            });

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"o sistema deve retornar status 201 Created")]
        public void ThenDeveRetornar201Created()
        {
            Assert.Equal(System.Net.HttpStatusCode.Created, _response.StatusCode);
        }

        [Then(@"o usuário retornado deve conter nome e e-mail iguais aos enviados")]
        public void ThenUsuarioDeveConterNomeEEmail()
        {
            var json = JObject.Parse(_response.Content!);
            Assert.Equal(_nome, json["nome"]?.ToString());
            Assert.Equal(_email, json["email"]?.ToString());
        }

        // Segundo Cenário: Tentar Criar um Usuário com um email já cadastrado
        [Given(@"que desejo cadastrar um e-mail já existente")]
        public async Task GivenQueDesejoCadastrarEmailExistente()
        {
            _client = RestClientFactory.Create();

            _emailTestado = RandomGenerator.GerarEmailUnico();

            //garante que o email já exista
            var request = new RestRequest("api/usuario", Method.Post);
            request.AddJsonBody(new
            {
                Nome = RandomGenerator.GerarNome(),
                Email = _emailTestado,
                Senha = RandomGenerator.GerarSenha(),
                Telefone = RandomGenerator.GerarTelefone(),
                UsuarioRole = "user"
            });

            var response = await _client.ExecuteAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [When(@"eu tento cadastrar um novo usuário com o mesmo e-mail")]
        public async Task WhenEuTentoCadastrarUmNovoUsuarioComOMesmoEmail()
        {
            var request = new RestRequest("api/usuario", Method.Post);
            request.AddJsonBody(new
            {
                Nome = "OutroNome",
                Email = _emailTestado,
                Senha = RandomGenerator.GerarSenha(),
                Telefone = RandomGenerator.GerarTelefone(),
                UsuarioRole = _role
            });

            _response = await _client.ExecuteAsync(request);
        }

        [Then(@"o sistema deve retornar status 400 Bad Request")]
        public void ThenSistemaDeveRetornar400BadRequest()
        {
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, _response.StatusCode);
        }

        [Then(@"a mensagem de erro deve ser ""(.*)""")]
        public void ThenMensagemDeErro(string mensagemEsperada)
        {
            var mensagem = _response.Content?.Trim('"'); 
            Assert.Equal(mensagemEsperada, mensagem);
        }

        // Tentar cadastrar um usuário, sem enviar os dados obrigatórios. Esse teste valida o status code com o Then já existente no arquivo -
        // "[Then(@"o sistema deve retornar status 400 Bad Request")]"
        [When(@"eu envio um usuário com nome ""(.*)"", e-mail ""(.*)"", senha ""(.*)"", telefone ""(.*)"" e role ""(.*)""")]
        public async Task WhenEnvioUsuarioComCamposVazios(string nome, string email, string senha, string telefone, string role)
        {
            _client = Utils.RestClientFactory.Create();

            var request = new RestRequest("api/usuario", Method.Post);
            request.AddJsonBody(new
            {
                Nome = nome,
                Email = email,
                Senha = senha,
                Telefone = telefone,
                UsuarioRole = role
            });

            _response = await _client.ExecuteAsync(request);
        }
    }
}
