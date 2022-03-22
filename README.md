# Keener challenge - Full Stack - Backend
## Desafio

O mesmo app do Mobile Front, com as orientações abaixo.
Você deverá construir uma api que tenha autenticação em uma das linguagens (C#, Python,
JavaScript), sendo que o app deve consumir a api. Sua API deve conter:
* Autenticação
* Produtos (listagem, adição, edição remoção)
* Compras (identificar quem comprou, lista de produtos na compra)
* Usuários (listagem, adição, edição e remoção)

## Sobre:
A API foi desenvolvida em C# com ASP.NET Core 6.0 junto com o Entity Framework Core (EF Core) como ORM, além do Swagger para a documentação da API que pode ser gerada dinamicamente. Um exemplo desta documentação está salva no repositório (swagger.json). Você pode utilizar o editor online para ler o arquivo de documentação: https://editor.swagger.io/. 

Para desenvolver essa API, utilizei alguns pacotes populares como:
* FluentValidation para validação dos dados dos requests;
* JwtBearer (Microsoft.AspNetCore.Authentication.JwtBearer) que habilita o suporte para autenticação baseada em portador Json Web Token (JWT);
* ASP.NET Identity para gerenciar os usuários e roles, além da autenticação dos usuários por e-mail e senha.
* AutoMapper para mapear e converter entidades para DTOs.

## Configuração:
O preenchimento de tabelas com dados iniciais ("Seed Data") com dois roles, um usuário administrador e três produtos será feito no momento que o banco e as tabelas forem criadas, pois o "DbContext.OnModelCreating" foi sobreposto.

### Roles:
* User
* Administrator

### Usuário padrão:
* E-mail de acesso: adm@test.dev
* Senha: $ABC12345wasd$

## Como utilizar

Baixe o projeto e execute "dotnet restore" para realizar o download dos pacotes utilizados.
As configurações da DB e JWT podem ser encontradas em "appsettings.json".
Você pode configurar as URL (http e https) em "Properties/launchSettings.json".

https://insomnia.rest/
https://www.postman.com/