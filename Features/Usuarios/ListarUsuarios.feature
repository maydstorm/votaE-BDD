Feature: Listar Usuários
  Como um administrador do sistema
  Eu quero visualizar todos os usuários cadastrados
  Para que eu possa gerenciar e acompanhar o sistema

Scenario: Listar todos os usuários com sucesso
  Given que estou autenticado com um token de administrador
  When eu solicito a lista de usuários
  Then o sistema deve retornar status 200 ao listar usuários
  And a resposta deve estar em conformidade com o schema de usuario

Scenario: Impedir listagem de usuários por usuário comum
  Given que estou autenticado com um token de usuário comum
  When eu solicito a lista de usuários
  Then o sistema deve me retornar status 403 Forbidden
