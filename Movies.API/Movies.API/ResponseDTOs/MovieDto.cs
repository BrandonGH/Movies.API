namespace Movies.API.ResponseDTOs;

public record MovieDto(
    Guid Id,
    DateOnly ReleaseDate,
    string Title,
    string Overview,
    decimal Popularity,
    int VoteCount,
    decimal VoteAverage,
    string OriginalLanguage,
    IEnumerable<string> Genre,
    string PosterUrl
    );