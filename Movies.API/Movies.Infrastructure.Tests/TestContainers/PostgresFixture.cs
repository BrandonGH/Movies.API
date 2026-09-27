using Testcontainers.PostgreSql;

namespace Movies.Infrastructure.Tests.TestContainers;

public class PostgresFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container =
        new PostgreSqlBuilder("postgres:18-alpine")
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .Build();

    public string ConnectionString => _container.GetConnectionString();

    public ValueTask InitializeAsync() => new(_container.StartAsync());

    public ValueTask DisposeAsync()
    {
        // Satisfies warning CA1816 (Dispose methods should call SuppressFinalize)
        GC.SuppressFinalize(this);
        return new ValueTask(_container.DisposeAsync().AsTask());
    }
}