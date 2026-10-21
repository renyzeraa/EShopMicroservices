# Basket.Api

Manages shopping carts, applies discounts from Discount.Grpc and publishes the checkout event to RabbitMQ.

**Dependencies:** BasketDb (PostgreSQL), Redis, RabbitMQ and Discount.Grpc.

## With Docker

From the `src` folder:

```bash
docker compose up -d --build basket.api
```

This also starts BasketDb, Redis, RabbitMQ and Discount.Grpc. The API runs at http://localhost:6001 (HTTPS: https://localhost:6061).

## With the .NET SDK

Requires the .NET 10 SDK. From the `src` folder, start the dependencies and run the API:

```bash
docker compose up -d --build basketdb distribuitedcache messagebroker discount.grpc
dotnet run --project Services/Basket/Basket.Api --launch-profile https -- --GrpcSettings:DiscountUrl=https://localhost:6062
```

The API runs at https://localhost:5051 (HTTP: http://localhost:5001). Check `/health` to confirm it is running.

To use Discount.Grpc running with `dotnet run` instead of the container, omit the `--GrpcSettings:DiscountUrl` argument (the default is https://localhost:5052).
