# Configuration

This document describes the configuration required to run My Next Spot locally and the settings used by the API in deployed environments.

## Prerequisites

- .NET 10 SDK
- Azure Functions Core Tools v4
- Azure Cosmos DB account or local emulator
- Credentials for the external services used by the features you want to run

## API Configuration

The API loads configuration from the .NET configuration system and binds the root configuration to the `Configurations` class.

For local development, the API loads `NS.API/local.settings.json` and .NET User Secrets. User Secrets are loaded after the JSON file and therefore take precedence when the same key is defined in both sources.

### Recommended local setup

From the repository root, initialize or confirm the User Secrets store:

```bash
dotnet user-secrets init --project NS.API
```

Add settings using hierarchical keys. For example:

```bash
dotnet user-secrets set "CosmosDB:DatabaseId" "your-database-id" --project NS.API
dotnet user-secrets set "CosmosDB:ConnectionString" "your-cosmos-db-connection-string" --project NS.API
```

Add the remaining settings required by the services you intend to use. Do not commit credentials, API keys, signing secrets, or connection strings.

### API settings reference

| Configuration key | Purpose |
| --- | --- |
| `CosmosDB:DatabaseId` | Cosmos DB database identifier |
| `CosmosDB:ConnectionString` | Cosmos DB connection string |
| `Apple:Endpoint` | Apple App Store server API endpoint |
| `Apple:SharedSecret` | Apple subscription shared secret |
| `Apple:BundleId` | Apple application bundle identifier |
| `Apple:Premium:PriceMonth` | Monthly Apple premium product identifier |
| `Apple:Premium:PriceYear` | Yearly Apple premium product identifier |
| `Stripe:ApiKey` | Stripe secret API key |
| `Stripe:SigningSecret` | Stripe webhook signing secret |
| `Stripe:Premium:PriceMonth` | Monthly Stripe price identifier |
| `Stripe:Premium:PriceYear` | Yearly Stripe price identifier |
| `RapidAPI:Key` | RapidAPI key |
| `Parsehub:Key` | ParseHub API key |
| `Scraping:Sherpa` | Scraping service credential |
| `ClerkAuth:SecretKey` | Clerk backend secret key |
| `ClerkAuth:SigningSecret` | Clerk webhook signing secret |
| `UserJot:ProjectId` | UserJot project identifier |
| `UserJot:Secret` | UserJot secret |
| `ZeptoMail:ApiKey` | ZeptoMail API key |
| `ZeptoMail:SecrectWebhook` | ZeptoMail webhook secret, using the property name currently defined by the application |
| `ZeptoMail:JobApiKey` | ZeptoMail job API key |
| `IsMaintenanceMode` | Enables or disables maintenance mode |

Some integrations are only needed when their corresponding functionality is used. Configure the credentials and product identifiers for the services enabled in your environment.

For deployed environments, provide the same keys through the hosting platform's application settings or environment variables. When using environment variables, replace the colon in hierarchical keys with a double underscore. For example, use `CosmosDB__ConnectionString` for `CosmosDB:ConnectionString`.

### Azure Functions local settings

The committed `NS.API/local.settings.json` contains the local Functions host settings:

- `AzureWebJobsStorage`: storage connection used by the Functions host. The current default is `UseDevelopmentStorage=true`, which requires the Azure Storage emulator, such as Azurite.
- `FUNCTIONS_WORKER_RUNTIME`: set to `dotnet-isolated`.

Keep machine-specific values and secrets out of source control. The Functions host settings belong under the `Values` object in `local.settings.json`. Application settings should be supplied through User Secrets or the deployed application's configuration provider.

## Web Application Configuration

The Blazor WebAssembly application uses `NS.WEB/wwwroot/appsettings.Development.json` to configure the API address during local development.

The current local setting is:

```json
{
  "ApiBaseAddress": "http://localhost:7262/api/"
}
```

The API project is configured to start on port `7262` through `NS.API/Properties/launchSettings.json`. If you change the API port, update `ApiBaseAddress` accordingly.

In non-local environments, the web application derives the API address from the web application's base address and appends `api/`. No production `ApiBaseAddress` value is required by the current implementation.

Do not place server-side secrets in Blazor WebAssembly configuration. Files under `wwwroot` are downloaded by the browser and their contents are public.

## Running Locally

1. Restore the solution:

   ```bash
   dotnet restore NS.sln
   ```

2. Configure the API credentials using User Secrets.

3. Start the API from the `NS.API` directory:

   ```bash
   func start
   ```

4. In a separate terminal, start the web application:

   ```bash
   dotnet run --project NS.WEB
   ```

5. Open the local URL printed by the web application.

## Deployment Notes

- Configure API secrets in the hosting environment, not in the repository.
- Ensure the production Cosmos DB connection string and database identifier are configured.
- Configure payment, authentication, email, data-provider, and scraping credentials when those integrations are enabled.
- Configure the Functions host storage setting for the deployed environment.
- The web application calls the API under the same origin using the `api/` path outside local development.
