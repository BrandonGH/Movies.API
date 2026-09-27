using Microsoft.AspNetCore.Mvc;
using Movies.API.ResponseDTOs;
using Movies.Core;
using Movies.Domain.Movies;

namespace Movies.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController(IMovieRepository movieRepository) : ControllerBase
{
    private IMovieRepository  _movieRepository = movieRepository;

    [HttpGet(Name = "GetMovies")]
    public IEnumerable<MovieDto> GetMovies()
    {
        var movies = _movieRepository.GetMovies();

        return movies.Select(x => new MovieDto(
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