Feature: Listar Sugestões
  Como um usuário autenticado
  Eu quero visualizar todas as sugestões cadastradas
  Para que eu possa acompanhar as propostas da comunidade

Scenario: Listar todas as sugestões com sucesso
  Given que estou autenticado com um token válido de usuário
  When eu solicito a lista de sugestões
  Then o sistema deve retornar status 200 ao listar sugestões
  And a resposta deve estar em conformidade com o schema de sugestao

Scenario: Impedir listagem de sugestões sem autenticação
  Given que não estou autenticado
  When eu solicito a lista de sugestões
  Then o sistema deve me retornar status 401 Unauthorized
