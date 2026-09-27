# Movies.API

ASP.NET Core backend for the Movies app. The API exposes movie search, filtering, sorting, and server-side pagination endpoints, and it uses PostgreSQL for storage. The API base URL is `http://localhost:5043`.

## Run the API directly

```bash
dotnet restore
dotnet run --project Movies.API/Movies.API/Movies.API.csproj
```

The API is available at `http://localhost:5043/` and the Scalar docs are available at `http://localhost:5043/scalar/v1`.
Configure the API to allow CORS requests from `http://localhost:4200`.

## Build and run with Docker

```bash
docker build -t movies.api:dev -f Movies.API/Movies.API/Dockerfile .
docker run --rm -p 5043:8080 --name MoviesAPI movies.api:dev
```

The API is available at `http://localhost:5043/` and the Scalar docs are available at `http://localhost:5043/scalar/v1`.
Configure the API to allow CORS requests from `http://localhost:4200`.

# Run with client

The project Docker Compose file runs the API and frontend together, so you can use `docker compose up` from this project for the full setup.
