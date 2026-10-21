# YarpApiGateway

Single entry point that routes `/catalog-service`, `/basket-service` and `/ordering-service` to the corresponding APIs.

**Dependencies:** Catalog.Api, Basket.Api and Ordering.API.

## With Docker

From the `src` folder:

```bash
docker compose up -d --build yarpapigateway
```

This also starts the three APIs and their dependencies. The gateway runs at http://localhost:6004 (HTTPS: https://localhost:6064).

## With the .NET SDK

Requires the .NET 10 SDK. The default destinations use the container names, so point them to the APIs exposed on localhost. From the `src` folder:

```bash
docker compose up -d --build catalog.api basket.api ordering.api
dotnet run --project ApiGateways/YarpApiGateway --launch-profile https -- \
  --ReverseProxy:Clusters:catalog-cluster:Destinations:destination1:Address=http://localhost:6000 \
  --ReverseProxy:Clusters:basket-cluster:Destinations:destination1:Address=http://localhost:6001 \
  --ReverseProxy:Clusters:ordering-cluster:Destinations:destination1:Address=http://localhost:6003
```

The gateway runs at https://localhost:5054 (HTTP: http://localhost:5004).
