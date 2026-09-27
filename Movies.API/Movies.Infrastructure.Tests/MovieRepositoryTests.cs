using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Movies.Domain.Movies;
using Movies.Infrastructure.Context;
using Movies.Infrastructure.Data.Repositories;
using Movies.Infrastructure.Tests.TestContainers;

namespace Movies.Infrastructure.Tests;

[Collection("Postgres collection")]
public class MovieRepositoryTests : IDisposable
{
    private readonly MoviesContext _context;
    private readonly MovieRepository _repository;

    public MovieRepositoryTests(PostgresFixture fixture)
    {
        var options = new DbContextOptionsBuilder<MoviesContext>()
            .UseNpgsql(fixture?.ConnectionString)
            .Options;

        _context = new MoviesContext(options);
        
        _context.Database.Migrate();
        _context.Database.ExecuteSqlRaw("TRUNCATE TABLE \"Movies\" RESTART IDENTITY CASCADE;");
        
        _repository = new MovieRepository(_context);

        SeedTestData();
    }

    private void SeedTestData()
    {
        var movies = new List<Movie>
        {
            new Movie(
                Guid.NewGuid(),
                new DateOnly(2023, 1, 15),
                "The Dark Knight",
                "Batman fights crime in Gotham",
                95.5m,
                25000,
                8.9m,
                "en",
                ["Action", "Drama", "Crime"],
                "poster1.jpg"
            ),
            new Movie(
                Guid.NewGuid(),
                new DateOnly(2023, 3, 20),
                "Inception",
                "Dreams within dreams",
                88.3m,
                20000,
                8.8m,
                "en",
                ["Action", "Sci-Fi", "Thriller"],
                "poster2.jpg"
            ),
            new Movie(
                Guid.NewGuid(),
                new DateOnly(2022, 6, 10),
                "Amélie",
                "A whimsical story in Paris",
                72.1m,
                15000,
                8.3m,
                "fr",
                ["Romance", "Comedy"],
                "poster3.jpg"
            ),
            new Movie(
                Guid.NewGuid(),
                new DateOnly(2023, 5, 5),
                "The Matrix",
                "Reality is not what it seems",
                90.0m,
                22000,
                8.7m,
                "en",
                ["Action", "Sci-Fi"],
                "poster4.jpg"
            ),
            new Movie(
                Guid.NewGuid(),
                new DateOnly(2021, 11, 12),
                "Parasite",
                "Class warfare in Seoul",
                85.2m,
                18000,
                8.6m,
                "ko",
                ["Drama", "Thriller"],
                "poster5.jpg"
            ),
            new Movie(
                Guid.NewGuid(),
                new DateOnly(2023, 2, 14),
                "Interstellar",
                "Journey through space and time",
                92.8m,
                23000,
                8.6m,
                "en",
                ["Sci-Fi", "Drama", "Adventure"],
                "poster6.jpg"
            ),
            new Movie(
                Guid.NewGuid(),
                new DateOnly(2022, 8, 25),
                "The Godfather",
                "Mafia family saga",
                98.5m,
                30000,
                9.2m,
                "en",
                ["Crime", "Drama"],
                "poster7.jpg"
            ),
            new Movie(
                Guid.NewGuid(),
                new DateOnly(2023, 4, 18),
                "Spirited Away",
                "A girl's adventure in a spirit world",
                78.9m,
                16000,
                8.6m,
                "ja",
                ["Animation", "Fantasy", "Adventure"],
                "poster8.jpg"
            )
        };

        _context.Movies.AddRange(movies);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetMovies_WithNoFilters_ReturnsAllMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, null, null, null, null);

        // Assert
        Assert.Equal(8, result.TotalCount);
        Assert.Equal(8, result.Items.Count);
    }

    [Fact]
    public async Task GetMovies_WithTitleSearch_ReturnsMatchingMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, "the", null, null, null, null);

        // Assert
        Assert.Equal(3, result.TotalCount); // The Dark Knight, The Matrix, The Godfather
        Assert.All(result.Items, movie => Assert.Contains("the", movie.Title, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetMovies_WithSingleGenre_ReturnsMatchingMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, ["Sci-Fi"], null, null, null);

        // Assert
        Assert.Equal(3, result.TotalCount); // Inception, The Matrix, Interstellar
        Assert.All(result.Items, movie => Assert.Contains("Sci-Fi", movie.Genre));
    }

    [Fact]
    public async Task GetMovies_WithMultipleGenres_ReturnsMoviesMatchingAllGenres()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, ["Action", "Crime"], null, null, null);

        // Assert
        Assert.Equal(1, result.TotalCount); // The Dark Knight
        Assert.All(result.Items, movie =>
            Assert.True(movie.Genre.Contains("Action") && movie.Genre.Contains("Crime")));
    }

    [Fact]
    public async Task GetMovies_WithLanguageFilter_ReturnsMatchingMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, null, "en", null, null);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.All(result.Items, movie => Assert.Equal("en", movie.OriginalLanguage));
    }

    [Fact]
    public async Task GetMovies_WithLanguageFilter_French_ReturnsOnlyFrenchMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, null, "fr", null, null);

        // Assert
        Assert.Single(result.Items); // Only Amélie
        Assert.Equal("Amélie", result.Items[0].Title);
    }

    [Fact]
    public async Task GetMovies_SortByTitle_Ascending_ReturnsSortedMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, null, null, "Title", "asc");

        // Assert
        Assert.Equal(8, result.TotalCount);
        Assert.Equal("Amélie", result.Items[0].Title);
        Assert.Equal("The Matrix", result.Items.Last().Title);
    }

    [Fact]
    public async Task GetMovies_SortByTitle_Descending_ReturnsSortedMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, null, null, "Title", "desc");

        // Assert
        Assert.Equal(8, result.TotalCount);
        Assert.Equal("The Matrix", result.Items[0].Title);
        Assert.Equal("Amélie", result.Items.Last().Title);
    }

    [Fact]
    public async Task GetMovies_SortByPopularity_Descending_ReturnsSortedMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, null, null, "Popularity", "desc");

        // Assert
        Assert.Equal(8, result.TotalCount);
        Assert.Equal("The Godfather", result.Items[0].Title);
        Assert.Equal("Amélie", result.Items.Last().Title);
    }

    [Fact]
    public async Task GetMovies_SortByReleaseDate_Descending_ReturnsSortedMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, null, null, "ReleaseDate", "desc");

        // Assert
        Assert.Equal(8, result.TotalCount);
        Assert.Equal("The Matrix", result.Items[0].Title);
        Assert.Equal("Parasite", result.Items.Last().Title);
    }

    [Fact]
    public async Task GetMovies_SortByVoteAverage_Descending_ReturnsSortedMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, null, null, "VoteAverage", "desc");

        // Assert
        Assert.Equal(8, result.TotalCount);
        Assert.Equal("The Godfather", result.Items[0].Title);
        Assert.Equal("Amélie", result.Items.Last().Title);
    }

    [Fact]
    public async Task GetMovies_WithPagination_FirstPage_ReturnsCorrectPage()
    {
        // Act
        var result = await _repository.GetMovies(1, 3, null, null, null, "Title", "asc");

        // Assert
        Assert.Equal(8, result.TotalCount);
        Assert.Equal(3, result.Items.Count);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(3, result.PageSize);
        Assert.Equal(3, result.TotalPages);
        Assert.True(result.HasNext);
        Assert.False(result.HasPrevious);
    }

    [Fact]
    public async Task GetMovies_WithPagination_SecondPage_ReturnsCorrectPage()
    {
        // Act
        var result = await _repository.GetMovies(2, 3, null, null, null, "Title", "asc");

        // Assert
        Assert.Equal(8, result.TotalCount);
        Assert.Equal(3, result.Items.Count);
        Assert.Equal(2, result.PageNumber);
        Assert.True(result.HasNext);
        Assert.True(result.HasPrevious);
    }

    [Fact]
    public async Task GetMovies_WithPagination_LastPage_ReturnsCorrectPage()
    {
        // Act
        var result = await _repository.GetMovies(3, 3, null, null, null, "Title", "asc");

        // Assert
        Assert.Equal(8, result.TotalCount);
        Assert.Equal(2, result.Items.Count); // Only 2 items on last page
        Assert.Equal(3, result.PageNumber);
        Assert.False(result.HasNext);
        Assert.True(result.HasPrevious);
    }

    [Fact]
    public async Task GetMovies_CombinedFilters_TitleAndGenre_ReturnsMatchingMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, "the", ["Action"], null, null, null);

        // Assert
        Assert.Equal(2, result.TotalCount); // The Dark Knight, The Matrix
        Assert.All(result.Items, movie =>
        {
            Assert.Contains("the", movie.Title, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Action", movie.Genre);
        });
    }

    [Fact]
    public async Task GetMovies_CombinedFilters_GenreAndLanguage_ReturnsMatchingMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, ["Drama"], "en", null, null);

        // Assert
        Assert.Equal(3, result.TotalCount); // The Dark Knight, Interstellar, The Godfather
        Assert.All(result.Items, movie =>
        {
            Assert.Contains("Drama", movie.Genre);
            Assert.Equal("en", movie.OriginalLanguage);
        });
    }

    [Fact]
    public async Task GetMovies_CombinedFilters_TitleGenreLanguageAndSort_ReturnsMatchingMovies()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, "the", ["Drama"], "en", "VoteAverage", "desc");

        // Assert
        Assert.Equal(2, result.TotalCount); // The Dark Knight, The Godfather
        Assert.Equal("The Godfather", result.Items[0].Title);
        Assert.Equal("The Dark Knight", result.Items[1].Title);
    }

    [Fact]
    public async Task GetMovies_CombinedFilters_WithPagination_ReturnsCorrectPage()
    {
        // Act
        var result = await _repository.GetMovies(1, 2, null, ["Action"], null, "Popularity", "desc");

        // Assert
        Assert.Equal(3, result.TotalCount); // The Dark Knight, Inception, The Matrix
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(2, result.TotalPages);
        Assert.True(result.HasNext);
    }

    [Fact]
    public async Task GetMovies_InvalidSortColumn_UsesDefaultSort()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, null, null, null, "InvalidColumn", null);

        // Assert
        Assert.Equal(8, result.TotalCount);
        // Should still return results without error
    }

    [Fact]
    public async Task GetMovies_NoMatches_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.GetMovies(1, 10, "NonExistentMovie", null, null, null, null);

        // Assert
        Assert.Equal(0, result.TotalCount);
        Assert.Empty(result.Items);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
