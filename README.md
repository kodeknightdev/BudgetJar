# Budget Jar

A small personal budgeting app: record income and expenses against categories and see how each month adds up on a chart.

## Stack

- **server/** - ASP.NET Core 3.1 Web API, Entity Framework Core 3.1, SQL Server
- **client/** - Angular 10, Chart.js 2

## Features

- Register / sign in (JWT bearer tokens)
- Income and expense categories with colours
- Add, edit and delete transactions; browse by month
- Dashboard with a monthly income vs. expenses bar chart and an expense breakdown by category

## Running locally

### API

Requires the .NET Core 3.1 SDK and a SQL Server instance (LocalDB works fine on Windows).

```bash
cd server/BudgetJar.Api
dotnet run
```

The API listens on http://localhost:5000. The connection string is `ConnectionStrings:BudgetJar` in `appsettings.json`.

The JWT signing key in `appsettings.json` is for local development only. Override it with an environment variable (`Jwt__Key`) or user secrets anywhere else.

#### Database

The schema is created on first start with `EnsureCreated()` - there are no migrations yet. After pulling a model change, drop the dev database and let it be recreated:

```bash
dotnet ef database drop
```

(or drop `BudgetJar` from SSMS / Azure Data Studio).

### Client

Requires Node 12+.

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
