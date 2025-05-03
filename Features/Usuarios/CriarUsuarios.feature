Feature: Cadastro de usuário
  Como um cidadão
  Eu quero me cadastrar no sistema
  Para poder enviar sugestões e votar em projetos

Scenario: Criar usuário com dados válidos e únicos
  Given que desejo criar um novo usuário com dados válidos e únicos
  When eu envio os dados para a API de cadastro
  Then o sistema deve retornar status 201 Created
  And o usuário retornado deve conter nome e e-mail iguais aos enviados

Scenario: Não permitir cadastro com e-mail já existente
  Given que desejo cadastrar um e-mail já existente
  When eu tento cadastrar um novo usuário com o mesmo e-mail
  Then o sistema deve retornar status 400 Bad Request
  And a mensagem de erro deve ser "Já existe um usuário com este e-mail."

Scenario: Não permitir cadastro com campos obrigatórios vazios
  When eu envio um usuário com nome "", e-mail "", senha "", telefone "" e role ""
  Then o sistema deve retornar status 400 Bad Request
