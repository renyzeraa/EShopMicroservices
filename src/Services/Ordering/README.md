# Ordering.API

Manages orders and creates them from the basket checkout event received through RabbitMQ. In the development environment, it applies the migrations and inserts sample data on startup.

**Dependencies:** OrderDb (SQL Server) and RabbitMQ.

## With Docker

From the `src` folder:

```bash
docker compose up -d --build ordering.api
```

This also starts OrderDb and RabbitMQ. The API runs at http://localhost:6003 (HTTPS: https://localhost:6063).

## With the .NET SDK

Requires the .NET 10 SDK. From the `src` folder, start the dependencies and run the API:

```bash
docker compose up -d orderdb messagebroker
dotnet run --project Services/Ordering/Ordering.API --launch-profile https
```

The API runs at https://localhost:5053 (HTTP: http://localhost:5003). Check `/health` to confirm it is running.
