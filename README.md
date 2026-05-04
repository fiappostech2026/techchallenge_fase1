# FCG — Tech Challenge Fase 1

API REST para gerenciamento de uma plataforma de jogos, desenvolvida como parte do Tech Challenge Fase 1 da FIAP.

## Sobre o Projeto

O **FCG (FIAP Cloud Games)** é uma aplicação backend que permite o cadastro de usuários, gerenciamento de jogos e controle de biblioteca pessoal. A autenticação é feita via JWT e a API segue os princípios REST.

## Tecnologias

- **.NET 10** — framework principal
- **Entity Framework Core 10** — ORM com migrations automáticas
- **SQLite** — banco de dados
- **JWT Bearer** — autenticação e autorização
- **Serilog** — logging estruturado em arquivo
- **Swagger / OpenAPI** — documentação interativa da API

## Estrutura do Projeto

```
FCG/
├── FCG.Web.Api      # Camada de apresentação (Controllers, Middlewares, Extensions)
├── FCG.Domain       # Regras de negócio (Entities, Interfaces, Services, Validators)
├── FCG.Infra        # Infraestrutura (EF Context, Repositories, Migrations, Auth)
└── FCG.Tests        # Testes unitários
```

## Como Rodar

**Pré-requisitos:** .NET 10 SDK

```bash
# Clone o repositório
git clone https://github.com/fiappostech2026/techchallenge_fase1.git

# Restaure as dependências
dotnet restore

# Execute a aplicação
dotnet run --project FCG.Web.Api
```

A API estará disponível em `https://localhost:5001`. O banco de dados e o usuário master são criados automaticamente na primeira execução.

## Endpoints Principais

| Recurso      | Rota                  |
|--------------|-----------------------|
| Usuários     | `/api/usuario`        |
| Jogos        | `/api/jogo`           |
| Biblioteca   | `/api/biblioteca`     |

A documentação completa dos endpoints está disponível via Swagger em `/swagger`.

## Documentação

- [Event Storming (Miro)](https://miro.com/app/board/uXjVHZTwibA=/)

## Testes

```bash
dotnet test
```
