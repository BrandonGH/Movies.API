# Movies.API

ASP.NET Core backend for the Movies app. The API exposes movie search, filtering, sorting, and server-side pagination endpoints, and it uses PostgreSQL for storage. The API base URL is `http://localhost:5043`.

The API applies any pending database migrations automatically on startup. PostgreSQL must be available and the `DefaultConnection` connection string must be configured.

## Run the API directly

```bash
dotnet restore
dotnet run --project Movies.API/Movies.API/Movies.API.csproj
```

The API is available at `http://localhost:5043/` and the Scalar docs are available at `http://localhost:5043/scalar/v1`.
Configure the API to allow CORS requests from `http://localhost:4200`.
The Development settings provide the local PostgreSQL connection string.

## Build and run with Docker

```bash
docker build -t movies.api:dev -f Movies.API/Movies.API/Dockerfile .
docker run --rm -p 5043:5043 \
	-e 'ConnectionStrings__DefaultConnection=Host=host.docker.internal;Port=5432;Database=movies;Username=moviesadmin;Password=d0rR17T0JfuZ;' \
	--name MoviesAPI movies.api:dev
```

The API is available at `http://localhost:5043/` and the Scalar docs are available at `http://localhost:5043/scalar/v1`.
Configure the API to allow CORS requests from `http://localhost:4200`.

# Run with client

The project Docker Compose file starts PostgreSQL, waits for it to become healthy, and then starts the API and frontend. The API applies migrations automatically when it starts.

```bash
docker compose up
```
