namespace Movies.API.Requests;

public record MoviesFilter(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    IEnumerable<string>? Genres = null,
    string? Language = null,
    string? SortColumn = null,
    string? SortDirection = "asc"
);