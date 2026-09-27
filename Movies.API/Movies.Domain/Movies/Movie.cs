namespace Movies.Domain.Movies;

public sealed class Movie
{
    public Guid Id { get; init; }
    public DateOnly ReleaseDate { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Overview { get; init; } = string.Empty;
    public decimal Popularity { get; init; }
    public int VoteCount { get; init; }
    public decimal VoteAverage { get; init; }
    public string OriginalLanguage { get; init; } = string.Empty;
    public IEnumerable<string> Genre { get; init; } = new List<string>();
    public string PosterUrl { get; init; } = string.Empty;

    public Movie() { }

    public Movie(
        Guid id,
        DateOnly releaseDate,
        string title,
        string overview,
        decimal popularity,
        int voteCount,
        decimal voteAverage,
        string originalLanguage,
        IEnumerable<string> genre,
        string posterUrl)
    {
        Id = id;
        ReleaseDate = releaseDate;
        Title = title;
        Overview = overview;
        Popularity = popularity;
        VoteCount = voteCount;
        VoteAverage = voteAverage;
        OriginalLanguage = originalLanguage;
        Genre = genre;
        PosterUrl = posterUrl;
    }
}