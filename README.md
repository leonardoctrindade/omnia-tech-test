# 🍻 Developer Evaluation Project - API de Vendas (OMNIA)

Este projeto foi construído para avaliação técnica focada na construção de uma API RESTful completa e madura. Ele atende ao caso de uso da "DeveloperStore" utilizando o ecossistema .NET 8.

## 🚀 Tecnologias Utilizadas

- **.NET 8.0**
- **Entity Framework Core (PostgreSQL)**
- **MediatR (Padrão CQRS e Mediator)**
- **AutoMapper** (Mapeamento de DTOs)
- **FluentValidation** (Validação fail-fast na entrada da API)
- **xUnit, Bogus e NSubstitute** (Testes Unitários)
- **Docker Compose** (Containerização do Banco de Dados e Cache)
- **Swagger** (Documentação Interativa da API)
- **Event-Driven via MediatR (DIFERENCIAL IMPLEMENTADO)**: Publicação e log de eventos (`SaleCreated`, `SaleModified`, `SaleCancelled`, `ItemCancelled`).

## 💼 Regras de Negócios Implementadas (DDD)

A aplicação de negócio foca no Domínio de **Vendas (Sales)**, com as seguintes regras matemáticas restritas, validadas profundamente no `Domain`:

1. Compras com quantidade abaixo de **4 itens**: Nenhum desconto é aplicado.
2. Compras com **4 a 9 itens** idênticos: Recebem **10% de desconto**.
3. Compras com **10 a 20 itens** idênticos: Recebem **20% de desconto**.
4. Limite de Venda: **Não é possível** vender mais de 20 itens idênticos por produto na mesma venda.

*Nota: O Domínio está blindado contra adições parciais. Mesmo adicionando itens aos poucos, se o acumulado na venda do mesmo produto ultrapassar 20, o Domínio bloqueará e lançará exceção.*

## 🏗 Arquitetura

O projeto adota os princípios de **Clean Architecture e Domain-Driven Design (DDD)**:

- **Domain:** Modelagem Rica (`Sale`, `SaleItem`), validações de negócio, enums e contratos de repositório (`ISaleRepository`).
- **Application:** Casos de Uso manipulados como Commands/Queries pelo `MediatR` (`CreateSale`, `UpdateSale`, `GetSale`, `DeleteSale`).
- **ORM (Infra):** Implementação concreta dos repositórios e Mapeamentos do EF Core.
- **IoC:** Módulo isolado focado no contêiner de Injeção de Dependência para deixar a WebApi o mais limpa possível.
- **WebApi:** Controladores, tratamento de rotas e perfis de conversão Request/Response.

## ⚙️ Como Executar o Projeto Localmente

### 1. Requisitos
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) rodando na máquina.
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### 2. Subindo as Dependências (Bancos de Dados)
Na raiz do diretório do backend (onde está o arquivo `docker-compose.yml`), rode o seguinte comando para subir o PostgreSQL, MongoDB e o Redis:

```bash
docker-compose up -d
```

### 3. Rodando as Migrations (Opcional - caso não rode no start)
Para criar as tabelas base, vá para o diretório `src/Ambev.DeveloperEvaluation.WebApi/` e execute:
```bash
dotnet ef database update --project ../Ambev.DeveloperEvaluation.ORM
```

### 4. Executando a API
Ainda na raiz do `backend/src/Ambev.DeveloperEvaluation.WebApi`, execute:
```bash
dotnet run
```
**Acesse o Swagger no seu navegador local:** `https://localhost:xxxx/swagger` para testar os endpoints interativamente.

## 🧪 Testes

A suíte de testes unitários validam 100% das regras de negócio do Domínio descritas acima.

Para rodar os testes, acesse o diretório raiz e execute:
```bash
dotnet test
```

## 📋 Endpoints Disponíveis
A API oferece as rotas padrão CRUD via `SalesController`:

- `POST /api/Sales` - Cria uma venda inteira com itens e já processa limites/descontos.
- `GET /api/Sales/{id}` - Obtém a Venda detalhada com o total atualizado e itens com ou sem descontos.
- `PUT /api/Sales/{id}` - Modifica os cabeçalhos de uma Venda (Cliente, Branch).
- `DELETE /api/Sales/{id}` - Exclui e/ou marca a Venda como cancelada.

*Todos os endpoints fornecem um ApiResponse normalizado na saída.*

## 🤝 Considerações da Entrega

**Diferencial Implementado:** A publicação de eventos (`SaleCreated`, `SaleModified`, `SaleCancelled`, `ItemCancelled`) foi entregue de forma nativa utilizando as Notificações do `MediatR` acopladas ao sistema de Log (conforme permitido pelas orientações de diferencial do teste).

Caso houvesse mais tempo para escalar o desenvolvimento, as próximas melhorias incluiriam:
1. Conectar os eventos existentes a um Message Broker Real (RabbitMQ via Rebus) para processamento assíncrono entre microsserviços.
2. Adição de Testes de Integração varrendo do Controller ao Banco no contêiner local usando a biblioteca `Testcontainers`.
3. Maior granularidade no Update de Venda para manipular individualmente a edição de quantidades nos itens.