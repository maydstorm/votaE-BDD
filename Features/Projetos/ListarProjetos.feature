Feature: Listar Projetos
  Como um usuário autenticado
  Eu quero visualizar todos os projetos cadastrados
  Para que eu possa conhecer e votar nas melhores propostas

Scenario: Listar todos os projetos com sucesso
  Given que estou autenticado 
  When eu solicito a lista de projetos
  Then o sistema deve retornar status 200 ao listar projetos
  And a resposta deve estar em conformidade com o schema de listagem de projetos

Scenario: Impedir listagem de projetos sem autenticação
  Given que não estou autenticado com um token
  When eu solicito a lista de projetos
  Then o sistema deve retornar status 401 Unauthorized
