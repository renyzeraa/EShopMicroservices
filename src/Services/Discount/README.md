# Discount.Grpc

Manages per-product discount coupons, exposed via gRPC. It applies the SQLite migrations on startup.

**Dependencies:** none (SQLite file database).

## With Docker

From the `src` folder:

```bash
docker compose up -d --build discount.grpc
```

The service runs at http://localhost:6002 (HTTPS: https://localhost:6062).

## With the .NET SDK

Requires the .NET 10 SDK. From the `src` folder:

```bash
dotnet run --project Services/Discount/Discount.Grpc --launch-profile https
```

The service runs at https://localhost:5052 (HTTP: http://localhost:5002). It must be called with a gRPC client, using the contract in [discount.proto](Discount.Grpc/Protos/discount.proto).
