# Catalog.Api

Manages the product catalog. In the development environment, it inserts sample products on startup.

**Dependencies:** CatalogDb (PostgreSQL).

## With Docker

From the `src` folder:

```bash
docker compose up -d --build catalog.api
```

This also starts CatalogDb. The API runs at http://localhost:6000 (HTTPS: https://localhost:6060).

## With the .NET SDK

Requires the .NET 10 SDK. From the `src` folder, start the database and run the API:

```bash
docker compose up -d catalogdb
dotnet run --project Services/Catalog/Catalog.Api --launch-profile https
```

The API runs at https://localhost:5050 (HTTP: http://localhost:5000). Check `/health` to confirm it is running.
