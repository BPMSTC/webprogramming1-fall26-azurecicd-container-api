# Brewers API

ASP.NET Core (.NET 10) API for Milwaukee Brewers players, stats and games. Data is stored in SQL Server (EF Core).
The API and SQL Server each run in their own Docker container.

## Run everything in Docker

1. Copy `.env.example` to `.env` and set a strong `SA_PASSWORD` (8+ characters with upper, lower, digit and symbol;
   avoid `$ ; ' "` because they break Docker Compose and the connection string). `.env` is git-ignored.
2. `docker compose up --build`
3. Open http://localhost:8080/api/players

On first start the API waits for SQL Server to be healthy, applies the EF migrations, and seeds the Brewers mock data
into any empty table. Later starts skip seeding. Data lives in the `brewers-sqldata` volume:

| Command | Data |
|---|---|
| `docker compose down` / `up` | kept |
| `docker compose down -v` | deleted (reseeded on next start) |

The SQL Server image is `mcr.microsoft.com/mssql/server:2025-latest`; running it accepts the SQL Server EULA
(`ACCEPT_EULA=Y`).

## Run the API from Visual Studio against the SQL container

```
docker compose up sql -d
dotnet user-secrets set "ConnectionStrings:BrewersDb" "Server=localhost,<SQL_HOST_PORT>;Database=BrewersDb;User Id=sa;Password=<SA_PASSWORD>;Encrypt=True;TrustServerCertificate=True" --project baseballfun
```

`SQL_HOST_PORT` defaults to 1433. If a local SQL Server already uses 1433, set `SQL_HOST_PORT=14330` (or similar) in `.env`.

Then run the `http` profile. Use `baseballfun/baseballfun.http` to try the endpoints.

## Configuration

| Setting (env var) | Purpose |
|---|---|
| `ConnectionStrings__BrewersDb` | SQL Server connection string. Required unless in-memory storage is chosen. |
| `Storage__Provider=InMemory` | Use throwaway in-memory storage instead of SQL Server. |
| `Database__InitializeOnStartup=false` | Skip migrations and seeding at startup. |

## Migrations

```
dotnet ef migrations add <Name> --project baseballfun --output-dir Data/Migrations
```
