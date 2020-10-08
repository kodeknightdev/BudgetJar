# Budget Jar

A small personal budgeting app: record income and expenses against categories and see how each month adds up on a chart.

## Stack

- **server/** - ASP.NET Core Web API on .NET 5, Entity Framework Core 5, SQL Server
- **client/** - Angular 11, Chart.js 2

The project started on .NET Core 3.1 / Angular 10 and was moved to .NET 5 / Angular 11 - see the "Migrate to .NET 5" and "Bump Angular to v11" commits.

## Features

- Register / sign in (JWT bearer tokens)
- Income and expense categories with colours
- Add, edit and delete transactions; browse by month
- Dashboard with a monthly income vs. expenses bar chart and an expense breakdown by category

## Running locally

### API

Requires the .NET 5 SDK (see `server/global.json`) and a SQL Server instance.

On macOS / Linux, start SQL Server in docker from the repo root:

```bash
docker-compose up -d
```

`appsettings.Development.json` points at that container. On Windows with LocalDB, remove the connection string from `appsettings.Development.json` (or override `ConnectionStrings__BudgetJar`) to fall back to the LocalDB one in `appsettings.json`.

```bash
cd server/BudgetJar.Api
dotnet run
```

The API listens on http://localhost:5000 and Swagger UI is at http://localhost:5000/swagger in Development. The connection string is `ConnectionStrings:BudgetJar` in `appsettings.json`.

The JWT signing key in `appsettings.json` is for local development only. Override it with an environment variable (`Jwt__Key`) or user secrets anywhere else.

#### Database

The schema is created on first start with `EnsureCreated()` - there are no migrations yet. After pulling a model change, drop the dev database and let it be recreated:

```bash
dotnet ef database drop
```

(or drop `BudgetJar` from SSMS / Azure Data Studio).

### Client

Requires Node 12.14+ or 14.15+.

```bash
cd client
npm install
npm start
```

Open http://localhost:4200. Requests to `/api` are proxied to the API (`client/proxy.conf.json`), so no CORS setup is needed in development.

## API

| Method     | Route                                  |                                   |
|------------|----------------------------------------|-----------------------------------|
| POST       | `/api/auth/register`                   | create account, returns a token   |
| POST       | `/api/auth/login`                      | returns a token                   |
| GET/POST   | `/api/categories`                      | list / create                     |
| PUT/DELETE | `/api/categories/{id}`                 | update / delete                   |
| GET/POST   | `/api/transactions?year=&month=`       | list / create                     |
| PUT/DELETE | `/api/transactions/{id}`               | update / delete                   |
| GET        | `/api/summary/monthly?year=`           | income and expenses per month     |
| GET        | `/api/summary/categories?year=&month=` | expense totals per category       |

Everything except `/api/auth/*` and `/api/ping` needs an `Authorization: Bearer <token>` header.

## Troubleshooting

- **`npm start` shows proxy errors (ECONNREFUSED)** - the API isn't running on port 5000.
- **Login works but every page bounces back to /login** - check the clock on the machine; tokens are validated with a 1 minute skew.
- **`Invalid column name 'UserId'` / similar SQL errors** - the dev database predates a model change. Drop it (see *Database* above).
- **Chart area is blank** - there are no transactions for the selected year yet.
