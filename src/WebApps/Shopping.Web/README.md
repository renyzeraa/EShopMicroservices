# Shopping.Web

Storefront web app: products, cart, checkout and orders. It consumes the APIs through YarpApiGateway.

**Dependencies:** YarpApiGateway (and the APIs behind it).

## With Docker

From the `src` folder:

```bash
docker compose up -d --build shopping.web
```

This also starts the gateway, the APIs and their dependencies. The app runs at http://localhost:6005 (HTTPS: https://localhost:6065).

## With the .NET SDK

Requires the .NET 10 SDK. From the `src` folder, start the gateway and run the app:

```bash
docker compose up -d --build yarpapigateway
dotnet run --project WebApps/Shopping.Web --launch-profile https
```

The app runs at https://localhost:5055 (HTTP: http://localhost:5005). In development it calls the gateway at http://localhost:6004, set in [appsettings.Development.json](appsettings.Development.json).
