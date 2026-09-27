using Microsoft.AspNetCore.Mvc;
using Movies.API.Requests;
using Movies.API.Responses;
using Movies.Core;
using Movies.Domain.Movies;

namespace Movies.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController(IMovieRepository movieRepository) : ControllerBase
{
    [HttpGet(Name = "GetMovies")]
    public async Task<PagedList<MovieDto>> GetMovies([FromQuery] MoviesFilter filter)
    {
        var pagedMovies = await movieRepository.GetMovies(
            filter.PageNumber,
            filter.PageSize,
            filter.TitleSearchTerm,
            filter.Genres,
            filter.Language,
            filter.SortColumn,
            filter.SortDirection);

        return pagedMovies.Transform(x => new MovieDto(
            x.Id,
            x.ReleaseDate,
            x.Title,
            x.Overview,
            x.Popularity,
            x.VoteCount,
            x.VoteAverage,
            x.OriginalLanguage,
            x.Genre,
            x.PosterUrl
        ));
    }
}