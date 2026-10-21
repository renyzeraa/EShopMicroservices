# EShop Microservices

A reference e-commerce project in **.NET 10**, built for studying and practicing microservices architecture.

## Services

| Service | Responsibility |
|---|---|
| Catalog.Api | Manages the product catalog. |
| Basket.Api | Manages shopping carts, applies discounts and handles checkout. |
| Discount.Grpc | Manages per-product discount coupons via gRPC. |
| Ordering.API | Manages orders, created from the basket checkout. |
| YarpApiGateway | Single entry point that routes requests to the APIs. |
| Shopping.Web | Storefront web app: products, cart, checkout and orders. |

## Technologies

- .NET 10, ASP.NET Core Minimal APIs, Carter and Razor Pages
- YARP (API Gateway), gRPC and RabbitMQ with MassTransit
- MediatR, FluentValidation and Mapster
- PostgreSQL with Marten, SQL Server and SQLite with Entity Framework Core
- Redis
- Docker and Docker Compose

## Running locally

Requires Docker and Docker Compose. The containers use an HTTPS development certificate mounted from `~/.aspnet/https` (`eshop.pem` and `eshop.key`).

```bash
cd src
docker compose up -d --build --wait
```

| Service | URL |
|---|---|
| Shopping.Web | http://localhost:6005 |
| YarpApiGateway | http://localhost:6004 |
| Catalog | http://localhost:6000 |
| Basket | http://localhost:6001 |
| Discount (gRPC) | http://localhost:6002 |
| Ordering | http://localhost:6003 |

To stop the services, run `docker compose stop`.

To run a single service, see the README in its folder: [Catalog](src/Services/Catalog/README.md), [Basket](src/Services/Basket/README.md), [Discount](src/Services/Discount/README.md), [Ordering](src/Services/Ordering/README.md), [YarpApiGateway](src/ApiGateways/YarpApiGateway/README.md) and [Shopping.Web](src/WebApps/Shopping.Web/README.md).
