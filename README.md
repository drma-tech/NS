# My Next Spot

[![FOSSA License Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fdrma-tech%2FNS.svg?type=shield&issueType=license)](https://app.fossa.com/projects/git%2Bgithub.com%2Fdrma-tech%2FNS?ref=badge_shield&issueType=license)
[![FOSSA Security Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fdrma-tech%2FNS.svg?type=shield&issueType=security)](https://app.fossa.com/projects/git%2Bgithub.com%2Fdrma-tech%2FNS?ref=badge_shield&issueType=security)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=drma-tech_NS&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=drma-tech_NS)

**Find the best cities and countries to live or travel.**

Website: [my-next-spot.com](https://my-next-spot.com/en)

## Project Overview

My Next Spot helps people discover and compare destinations around the world. It brings together rankings, statistics, and practical information about countries and cities to support decisions about where to travel or live.

The platform combines destination discovery with indicators covering areas such as safety, cost of living, economy, quality of life, mobility, and tourism.

## Key Features

- **Destination Rankings:** Explore selected destinations using global, safety, and cost-related scores.
- **City Rankings:** Discover the world's most visited cities and global cities classified by their level of international importance.
- **Destination Explorer:** Browse countries and regions by continent, subcontinent, name, score, or map.
- **Country Comparisons:** Compare destinations across economic, social, safety, environmental, mobility, and cost-of-living indicators.
- **Destination Insights:** Access country information, including news, weather, holidays, conflicts, languages, and travel-related risks.
- **Curated Suggestions:** Browse destination ideas based on interests, events, festivals, and travel themes.
- **Travel Planning:** Save destinations to wishlists and next-destination lists, and keep a travel history.
- **Travel Resources:** Find links to accommodation, transportation, and other travel-related services.

## Technology Stack

- **Frontend:** Blazor WebAssembly, .NET 10, MudBlazor
- **Backend:** Azure Functions v4, .NET 10 isolated worker
- **Database:** Azure Cosmos DB
- **Authentication:** Microsoft Authentication Library (MSAL), Clerk
- **Payments:** Stripe, Apple In-App Purchases
- **Maps:** MapLibre GL, MapTiler
- **Monitoring:** Sentry
- **Feedback:** UserJot
- **Hosting and Deployment:** Azure Static Web Apps, GitHub Actions

## Architecture

The solution is organized into three projects:

| Project | Responsibility |
| --- | --- |
| `NS.WEB` | Blazor WebAssembly application and user interface |
| `NS.API` | Azure Functions backend, integrations, and data access |
| `NS.Shared` | Shared models, enums, helpers, and translations |

The frontend and backend reference `NS.Shared` to reuse common contracts and application logic.

## Getting Started

### Prerequisites

- .NET 10 SDK
- Azure Functions Core Tools v4
- Azure Cosmos DB account or local emulator
- Credentials for the external services required by the application

### Restore dependencies

```bash
dotnet restore NS.sln
```

### Run the API

Configure the API settings first, then run the Functions project:

```bash
cd NS.API
func start
```

### Run the web application

Configure the local API address in `NS.WEB/wwwroot/appsettings.Development.json`, then run:

```bash
dotnet run --project NS.WEB
```

## Configuration

See [docs/CONFIGURATION.md](docs/CONFIGURATION.md) for local setup, API settings, environment variables, and deployment configuration.

## Roadmap

See [ROADMAP.md](ROADMAP.md) for planned product development.

## License

This project is licensed under the [Mozilla Public License 2.0](LICENSE.txt).
