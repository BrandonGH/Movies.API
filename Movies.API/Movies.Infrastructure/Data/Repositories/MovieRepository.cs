using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Movies.Core;
using Movies.Domain.Movies;
using Movies.Infrastructure.Context;

namespace Movies.Infrastructure.Data.Repositories;

public class MovieRepository(MoviesContext context) : IMovieRepository
{
    public async Task<PagedList<Movie>> GetMovies(
        int pageNumber,
        int pageSize,
        string? titleSearchTerm,
        IEnumerable<string>? genres,
        string? language,
        string? sortColumn,
        string? sortDirection)
    {
        var query = context.Movies.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(titleSearchTerm))
        {
            query = query.Where(m => m.Title.Contains(titleSearchTerm));
        }

        // Apply genres filter
        var genreList = genres?.ToList();
        if (genreList != null && genreList.Count != 0)
        {
            query = query.Where(m => genreList.Any(g => m.Genre.Contains(g)));
        }

        // Apply language filter
        if (!string.IsNullOrWhiteSpace(language))
        {
            query = query.Where(m => m.OriginalLanguage == language);
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(sortColumn))
        {
            query = ApplySorting(query, sortColumn, sortDirection);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply pagination
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<Movie>(items, totalCount, pageNumber, pageSize);
    }

    private static IQueryable<Movie> ApplySorting(IQueryable<Movie> query, string sortColumn, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        Expression<Func<Movie, object>> keySelector = sortColumn.ToLower() switch
        {
            "title" => m => m.Title,
            "releasedate" => m => m.ReleaseDate,
            "popularity" => m => m.Popularity,
            "votecount" => m => m.VoteCount,
            "voteaverage" => m => m.VoteAverage,
            "originallanguage" => m => m.OriginalLanguage,
            _ => m => m.Title
        };

        return isDescending
            ? query.OrderByDescending(keySelector)
            : query.OrderBy(keySelector);
    }
}
