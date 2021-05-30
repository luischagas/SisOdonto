# SisOdonto 

Para execução do projeto é necessário realizar o seguinte passo:

## Instalação

Criar um container do SQL Server:

```bash
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Dockersql123#' -e 'MSSQL_PID=Express' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu
```

## Criação da estrutura do banco de dados

Após a execução do procedimento acima, basta executar os seguintes para que o entity framework core crie as tabelas. 

```bash
update-database -context SisOdontoIdentityContext
```

```bash
update-database -context SisOdontoContext
```

## O que foi implementado?

- .NET 5
- ASP.NET MVC
- Arquitetura Limpa
- Entity Framework Core
- SQL Server
- Autenticação no sistema com Identity com JWT
- Template AdminLTE com Bootstrap
