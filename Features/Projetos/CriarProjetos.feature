Feature: Cadastro de projetos
  Como um administrador
  Eu quero cadastrar projetos com base em sugestões aprovadas
  Para que eles fiquem disponíveis para votação

@projeto_valido
Scenario: Criar projeto com dados válidos
  Given que estou autenticado com um token válido de admin
  When eu envio um projeto com dados fixos baseado em uma sugestão
  Then o sistema deve retornar status 201 ao cadastrar projeto
  And o projeto retornado deve conter os dados enviados
  And o projeto retornado deve conter um projetoId não nulo
  And a resposta deve estar em conformidade com o schema de projeto

Scenario: Não permitir cadastro de projeto sem sugestaoId
  Given que estou autenticado com um token válido de admin
  When eu envio um projeto sem informar o campo sugestaoId
  Then o sistema deve retornar status 400 ao cadastrar projeto

Scenario: Buscar projeto com ID inexistente
  Given que estou autenticado com um token válido de admin
  When eu buscar um projeto com Id 9999
  Then o sistema deve retornar status 404 ao buscar projeto
