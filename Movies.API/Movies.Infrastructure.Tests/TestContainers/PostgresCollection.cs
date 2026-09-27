namespace Movies.Infrastructure.Tests.TestContainers;

[CollectionDefinition("Postgres collection")]
public class PostgresCollection : ICollectionFixture<PostgresFixture> { }