# EShop Microservices

E-commerce de referência em **.NET 10**, construído com microsserviços independentes, **Vertical Slice Architecture** e **CQRS**. O objetivo é servir como base prática de arquitetura distribuída: cada serviço tem seu próprio banco, seu próprio ciclo de vida e nenhuma dependência de código com os demais — apenas os blocos de infraestrutura compartilhados.

## Stack

| Camada | Tecnologia |
|---|---|
| Runtime | .NET 10 / ASP.NET Core Minimal APIs |
| Roteamento | [Carter](https://github.com/CarterCommunity/Carter) (módulos de endpoint) |
| Mensageria interna | [MediatR](https://github.com/jbogard/MediatR) (CQRS + pipeline behaviors) |
| Validação | FluentValidation |
| Mapeamento | Mapster |
| Persistência | PostgreSQL 17 via [Marten](https://martendb.io/) (document store) |
| Observabilidade | HealthChecks (`AspNetCore.HealthChecks.NpgSql`) |
| Orquestração | Docker Compose |

## Arquitetura

```
┌─────────────────┐        ┌─────────────────┐
│   Catalog.Api   │        │   Basket.Api    │
│  (porta 6000)   │        │  (porta 5001)   │
└────────┬────────┘        └────────┬────────┘
         │                          │
   ┌─────▼─────┐              ┌─────▼─────┐
   │ catalogdb │              │ basketdb  │
   │ pg :5432  │              │ pg :5433  │
   └───────────┘              └───────────┘
              ╲              ╱
             ┌──────────────────┐
             │  BuildingBlocks  │
             │ CQRS · Behaviors │
             │    Exceptions    │
             └──────────────────┘
```

**Vertical Slice**: cada caso de uso é uma pasta autocontida com `Endpoint` + `Handler` + `Command/Query` + `Validator` no mesmo arquivo/diretório. Nada de camadas horizontais anêmicas — a mudança de uma feature fica confinada a uma pasta.

**Pipeline MediatR** (aplicado a todo comando/query):
1. `ValidationBehavior` — executa os validators do FluentValidation e falha rápido com `ValidationException`.
2. `LoggingBehavior` — log de entrada/saída e alerta de performance para requisições acima de 3s.

**Tratamento de erros**: `CustomExceptionHandler` centraliza as exceções e devolve sempre `application/problem+json` (RFC 7807), com `traceId` e o dicionário de `errors` de validação quando aplicável.

| Exceção | HTTP |
|---|---|
| `ValidationException`, `BadRequestException` | 400 |
| `NotFoundException` | 404 |
| demais | 500 |

## Estrutura

```
src/
├── BuildingBlocks/BuildingBlocks/     # Biblioteca compartilhada (sem regra de negócio)
│   ├── CQRS/                          # ICommand, IQuery e seus handlers
│   ├── Behaviors/                     # Validation e Logging
│   └── Exceptions/                    # Exceções + handler global
├── Services/
│   ├── Catalog/Catalog.Api/           # Produtos
│   │   ├── Products/<UseCase>/        # Uma pasta por slice
│   │   ├── Data/                      # Seed inicial (Marten)
│   │   └── Models/
│   └── Basket/Basket.Api/             # Carrinho de compras
│       ├── Basket/<UseCase>/
│       ├── Data/                      # BasketRepository
│       └── Models/
├── docker-compose.yml
└── docker-compose.override.yml
```

## Executando

### Docker Compose (recomendado)

```bash
cd src
docker compose up -d
```

Sobe `catalogdb`, `basketdb` e `catalog.api`. O `catalog.api` só inicia após o healthcheck do Postgres passar.

- Catalog API → http://localhost:6000
- Health check → http://localhost:6000/health

### Local (dotnet CLI)

Suba apenas os bancos e rode os serviços pela IDE ou CLI:

```bash
cd src
docker compose up -d catalogdb basketdb

dotnet run --project Services/Catalog/Catalog.Api   # http://localhost:5000
dotnet run --project Services/Basket/Basket.Api     # http://localhost:5001
```

As connection strings padrão de `appsettings.json` já apontam para `localhost:5432` (catalog) e `localhost:5433` (basket). Em ambiente de desenvolvimento o Catalog popula o banco automaticamente via `CatalogInitialData`.

## Licença

[MIT](LICENSE)
