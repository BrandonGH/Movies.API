using Movies.Core;

namespace Movies.Domain.Movies;

public interface IMovieRepository
{
   Task<PagedList<Movie>> GetMovies(
        int pageNumber,
        int pageSize,
        string? titleSearchTerm,
        IEnumerable<string>? genres,
        string? language,
        string? sortColumn,
        string? sortDirection);
}