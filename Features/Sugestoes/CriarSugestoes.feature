Feature: Cadastro de sugestão
  Como um cidadão autenticado
  Eu quero enviar uma sugestão de melhoria para meu bairro
  Para que a prefeitura possa avaliá-la e transformá-la em um projeto

Scenario: Enviar sugestão com dados válidos
  Given que estou autenticado com um token válido
  When eu envio uma sugestão com dados fixos
  Then o sistema deve retornar status 201 Created ao criar uma sugestão
  And a sugestão retornada deve conter os dados enviados

Scenario: Não permitir sugestão sem o campo Usuario
  Given que estou autenticado com um token válido
  When eu envio uma sugestão sem informar o campo Usuario
  Then o sistema deve retornar status 400 ao validar sugestão
