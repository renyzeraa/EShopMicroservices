# EShop Microservices

A reference e-commerce project in **.NET 10**, built for studying and practicing microservices architecture with **Vertical Slice Architecture**, **Clean Architecture**, **DDD** and **CQRS**.

## Services

| Service | Responsibility | Database |
|---|---|---|
| Catalog.Api | Creates, queries, updates and deletes products. | PostgreSQL (Marten) |
| Basket.Api | Stores, queries and deletes shopping carts, with Redis caching. Applies discounts by calling Discount.Grpc and publishes the checkout event. | PostgreSQL (Marten) + Redis |
| Discount.Grpc | Creates, queries, updates and deletes per-product discount coupons, exposed via gRPC. | SQLite (EF Core) |
| Ordering.API | Creates, updates, deletes and queries orders, with domain events. Creates orders from the basket checkout. | SQL Server (EF Core) |
| YarpApiGateway | Single entry point that routes requests to Catalog, Basket and Ordering, with rate limiting. | — |
| Shopping.Web | Razor Pages storefront: product list, cart, checkout and orders. Consumes the APIs through the gateway. | — |

Each service owns its own database. The APIs and their dependencies are orchestrated with Docker Compose.

### Service communication

```text
Shopping.Web ──HTTP──▶ YarpApiGateway ──▶ Catalog.Api / Basket.Api / Ordering.API

Basket.Api ──gRPC──────────────────────────▶ Discount.Grpc
Basket.Api ──RabbitMQ (BasketCheckoutEvent)──▶ Ordering.API
```

- **Synchronous (gRPC):** when a cart is stored, Basket.Api fetches the discount for each item from Discount.Grpc and deducts it from the price.
- **Asynchronous (RabbitMQ + MassTransit):** on checkout, Basket.Api publishes `BasketCheckoutEvent` and deletes the cart; Ordering.API consumes the event and creates the order.

### API Gateway

YARP routes by path prefix, removing the prefix before forwarding:

| Gateway path | Destination |
|---|---|
| `/catalog-service/{**}` | Catalog.Api |
| `/basket-service/{**}` | Basket.Api |
| `/ordering-service/{**}` | Ordering.API |

Rate limiting uses a fixed window of **5 requests every 10 seconds** per route; extra requests get `429 Too Many Requests`.

### Feature flags

Ordering.API uses `Microsoft.FeatureManagement`. The `OrderFullfilment` flag (disabled by default) controls whether an integration event is published after an order is created.

## Technologies

| Area | Technologies |
|---|---|
| APIs | .NET 10, ASP.NET Core Minimal APIs and Carter |
| Web app | ASP.NET Core Razor Pages and Refit |
| API Gateway | YARP with ASP.NET Core Rate Limiting |
| Synchronous communication | gRPC (Protobuf) |
| Asynchronous communication | RabbitMQ with MassTransit |
| Commands and queries | MediatR |
| Validation and mapping | FluentValidation and Mapster |
| Persistence | PostgreSQL 17 with Marten, SQLite and SQL Server 2022 with Entity Framework Core |
| Cache | Redis |
| Feature flags | Microsoft.FeatureManagement |
| Monitoring | Health checks and logging |
| Containers | Docker and Docker Compose |

## Architecture

- **Vertical Slice Architecture** (Catalog and Basket): code organized by use case.
- **Clean Architecture** (Ordering): `Domain`, `Application`, `Infrastructure` and `API` layers.
- **DDD** (Ordering): aggregates, entities, strongly typed Value Objects and domain events.
- **CQRS**: separation between write commands and read queries.
- **EF Core interceptors** (Ordering): automatically fill audit fields and dispatch domain events on save.
- **Decorator with Scrutor** (Basket): Redis-cached repository wrapping the PostgreSQL repository.
- **Integration events**: contracts shared through `BuildingBlocks.Messaging`.
- **BuildingBlocks**: shared components for CQRS, validation, logging, error handling and pagination.
- **Independent persistence**: each microservice manages its own data.

## Structure

```text
src/
├── ApiGateways/
│   └── YarpApiGateway/            # Reverse proxy and rate limiting
├── BuildingBlocks/
│   ├── BuildingBlocks/            # Shared components
│   └── BuildingBlocks.Messaging/  # Integration events and MassTransit setup
├── Services/
│   ├── Catalog/                   # Product catalog
│   ├── Basket/                    # Shopping cart
│   ├── Discount/                  # Discount coupons (gRPC)
│   └── Ordering/                  # Orders (Clean Architecture)
│       ├── Ordering.API/
│       ├── Ordering.Application/
│       ├── Ordering.Domain/
│       └── Ordering.Infrastructure/
├── WebApps/
│   └── Shopping.Web/              # Razor Pages storefront
├── docker-compose.yml
└── docker-compose.override.yml
```

## Running

With **Docker and Docker Compose available in WSL**, run the commands in the Linux terminal from the repository root. Installing the .NET SDK locally is not required for the containerized services.

The containers use an HTTPS development certificate mounted from `~/.aspnet/https` (`eshop.pem` and `eshop.key`).

To build the images and start the APIs, gateway, databases, Redis and RabbitMQ:

```bash
cd src
docker compose up -d --build --wait
```

| Service | HTTP | HTTPS |
|---|---|---|
| Catalog | http://localhost:6000 | https://localhost:6060 |
| Basket | http://localhost:6001 | https://localhost:6061 |
| Discount (gRPC) | http://localhost:6002 | https://localhost:6062 |
| Ordering | http://localhost:6003 | https://localhost:6063 |
| YarpApiGateway | http://localhost:6004 | https://localhost:6064 |

Open `/health` on Catalog, Basket and Ordering to check that they are running. Discount.Grpc must be accessed through a gRPC client, using the contract in [discount.proto](src/Services/Discount/Discount.Grpc/Protos/discount.proto).

| Infrastructure | Local port |
|---|---|
| CatalogDb (PostgreSQL) | 5432 |
| BasketDb (PostgreSQL) | 5433 |
| Redis | 6379 |
| OrderDb (SQL Server) | 1433 |
| RabbitMQ (AMQP) | 5672 |
| RabbitMQ Management | http://localhost:15672 (`guest` / `guest`) |

In the development environment, Ordering applies migrations and inserts seed data automatically on startup.

### Web app

Shopping.Web is not containerized yet. With the containers running, start it with the .NET 10 SDK:

```bash
cd src
dotnet run --project WebApps/Shopping.Web --launch-profile https
```

Open https://localhost:5055. The app calls the gateway at `https://localhost:6064`, configured in `ApiSettings:GatewayAddress` in [appsettings.json](src/WebApps/Shopping.Web/appsettings.json).

### Stopping the services

To stop all services while keeping the containers and data:

```bash
docker compose stop
```

To also remove the containers and the network, use `docker compose down`. Avoid `down -v`, as it deletes the volumes holding the database data.

## License

[MIT](LICENSE)
