# 🧪 VotaE - Testes Automatizados BDD

Projeto de testes automatizados utilizando **SpecFlow**, com foco na API do sistema **VotaE**, que permite o envio de sugestões e a votação em projetos para melhorias urbanas.

## 📚 Tecnologias utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/download)
- [SpecFlow](https://specflow.org/)
- [RestSharp](https://restsharp.dev/)
- [FluentAssertions](https://fluentassertions.com/)
- [Newtonsoft.Json](https://www.newtonsoft.com/json)
- [NJsonSchema](https://github.com/RicoSuter/NJsonSchema) – para validação de contrato via JSON Schema

---

## 📁 Estrutura de pastas

```
VotaE-BDD/
Features/
│
├── Projetos/
│   ├── CriarProjetos.feature
│   └── ListarProjetos.feature
│
├── Sugestoes/
│   ├── CriarSugestoes.feature
│   └── ListarSugestoes.feature
│
└── Usuarios/
    ├── CriarUsuarios.feature
    └── ListarUsuarios.feature

Hooks/
└── ProjetoHooks.cs

Schemas/
├── projeto.schema.json
├── projeto-lista.schema.json
├── sugestao-lista.schema.json
└── usuario-lista.schema.json

Steps/
│
├── Projetos/
│   ├── CriarProjetosSteps.cs
│   └── ListarProjetosSteps.cs
│
├── Sugestoes/
│   ├── CriarSugestoesSteps.cs
│   └── ListarSugestoesSteps.cs
│
└── Usuarios/
    ├── CriarUsuariosSteps.cs
    └── ListarUsuariosSteps.cs

Utils/
├── DadosHelper.cs
├── RandomGenerator.cs
├── RestClient.cs
└── TokenHelper.cs

```

---

## ✅ O que é testado

### ✅ Usuários
- Criação de novo usuário com dados randômicos
- Rejeição de usuários com e-mail duplicado
- Validação de campos obrigatórios
- Listagem apenas por usuários 

### ✅ Sugestões
- Criação de sugestões válidas
- Rejeição de sugestões sem o campo obrigatório `Usuario`
- Listagem de sugestões
- Validação de JSON Schema

### ✅ Projetos
- Criação de projeto baseado em sugestão existente (via Hook)
- Rejeição de criação sem sugestaoId
- Listagem de projetos aprovados
- Busca por projeto inexistente
- Validação de JSON Schema

---

## 🧪 Como executar os testes

1. Certifique-se de ter o **.NET 8 SDK** instalado.

2. Compile o projeto:

```bash
dotnet build
```

3. Execute os testes:

```bash
dotnet test
```

---

## 🔐 Autenticação

Os testes utilizam **tokens JWT** obtidos via endpoint de login da API. O helper `TokenHelper.cs` realiza a autenticação como:

- Usuário padrão (`GetTokenUserAsync`)
- Admin (`GetTokenAdminAsync`)

---

## 📦 Validação de contrato

Todos os testes que envolvem **validação de resposta** utilizam schemas localizados na pasta `Schemas/`. Os testes quebram se a resposta JSON da API não estiver de acordo com o schema esperado.

---

## 🧠 Convenções adotadas

- Os dados utilizados nos testes estão centralizados em `DadosHelper.cs`
- Hooks (`ProjetoHooks.cs`) são usados para preparar dados prévios (ex: sugestão antes de criar projeto)
- `ScenarioContext` é utilizado para compartilhar dados entre steps
- Nomenclatura padronizada para arquivos `.feature` e `.schema.json`
- Os dados randômicos são gerados pelo `RandomGenerator.cs`

---

---

## 📃 Execução com Docker

Caso deseje rodar os testes em um ambiente isolado, como CI/CD, é possível utilizar um Dockerfile simples:

## 📄 Exemplo de Dockerfile
```
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet build --no-restore

CMD ["dotnet", "test", "--no-build", "--logger:trx"]
```

## .dockerignore recomendado
```
bin/
obj/
.vscode/
*.user
*.suo
```

## Como usar
```
# Construa a imagem
docker build -t votae-bdd .

# Execute os testes
docker run --rm votae-bdd
```

