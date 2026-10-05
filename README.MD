# EShop Microservices

A reference e-commerce project in **.NET 10**, built for studying and practicing microservices architecture with **Vertical Slice Architecture**, **Clean Architecture**, **DDD** and **CQRS**.

## Services

| Service | Responsibility | Database |
|---|---|---|
| Catalog.Api | Creates, queries, updates and deletes products. | PostgreSQL (Marten) |
| Basket.Api | Stores, queries and deletes shopping carts, with Redis caching. Applies discounts by calling Discount.Grpc. | PostgreSQL (Marten) + Redis |
| Discount.Grpc | Creates, queries, updates and deletes per-product discount coupons, exposed via gRPC. | SQLite (EF Core) |
| Ordering *(in development)* | Creates, updates, deletes and queries orders, with domain events. | SQL Server (EF Core) |

Each service owns its own database. The APIs and their dependencies are orchestrated with Docker Compose.

### Service communication

```text
Basket.Api ──gRPC──▶ Discount.Grpc
```

When a cart is stored, Basket.Api fetches the discount for each item from Discount.Grpc and deducts it from the price.

## Technologies

| Area | Technologies |
|---|---|
| APIs | .NET 10, ASP.NET Core Minimal APIs and Carter |
| Synchronous communication | gRPC (Protobuf) |
| Commands and queries | MediatR |
| Validation and mapping | FluentValidation and Mapster |
| Persistence | PostgreSQL 17 with Marten, SQLite and SQL Server 2022 with Entity Framework Core |
| Cache | Redis |
| Monitoring | Health checks and logging |
| Containers | Docker and Docker Compose |

## Architecture

- **Vertical Slice Architecture** (Catalog and Basket): code organized by use case.
- **Clean Architecture** (Ordering): `Domain`, `Application`, `Infrastructure` and `API` layers.
- **DDD** (Ordering): aggregates, entities, strongly typed Value Objects and domain events.
- **CQRS**: separation between write commands and read queries.
- **EF Core interceptors** (Ordering): automatically fill audit fields and dispatch domain events on save.
- **Decorator with Scrutor** (Basket): Redis-cached repository wrapping the PostgreSQL repository.
- **BuildingBlocks**: shared components for CQRS, validation, logging, error handling and pagination.
- **Independent persistence**: each microservice manages its own data.

## Structure

```text
src/
├── BuildingBlocks/                # Shared components
├── Services/
│   ├── Catalog/                   # Product catalog
│   ├── Basket/                    # Shopping cart
│   ├── Discount/                  # Discount coupons (gRPC)
│   └── Ordering/                  # Orders (in development)
│       ├── Ordering.API/
│       ├── Ordering.Application/
│       ├── Ordering.Domain/
│       └── Ordering.Infrastructure/
├── docker-compose.yml
└── docker-compose.override.yml
```

## Running

With **Docker and Docker Compose available in WSL**, run the commands in the Linux terminal from the repository root. Installing the .NET SDK locally is not required for the containerized services.

To build the images and start the APIs, databases and Redis:

```bash
cd src
docker compose up -d --build --wait
```

| Service | HTTP | HTTPS |
|---|---|---|
| Catalog | http://localhost:6000 | https://localhost:6060 |
| Basket | http://localhost:6001 | https://localhost:6061 |
| Discount (gRPC) | http://localhost:6002 | https://localhost:6062 |

Open `/health` on Catalog and Basket to check that they are running. Discount.Grpc must be accessed through a gRPC client, using the contract in [discount.proto](src/Services/Discount/Discount.Grpc/Protos/discount.proto).

| Database | Local port |
|---|---|
| CatalogDb (PostgreSQL) | 5432 |
| BasketDb (PostgreSQL) | 5433 |
| Redis | 6379 |
| OrderDb (SQL Server) | 1433 |

### Ordering

The ordering service does not expose endpoints or have its own container yet; Docker Compose only starts the `orderdb` database. Running it requires the .NET 10 SDK:

```bash
cd src
docker compose up -d orderdb
dotnet run --project Services/Ordering/Ordering.API
```

In the development environment, migrations are applied and seed data is inserted automatically on startup.

### Stopping the services

To stop all services while keeping the containers and data:

```bash
docker compose stop
```

To also remove the containers and the network, use `docker compose down`. Avoid `down -v`, as it deletes the volumes holding the database data.

## License

[MIT](LICENSE)
