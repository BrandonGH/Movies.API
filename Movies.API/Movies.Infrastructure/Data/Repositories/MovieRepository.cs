using Movies.Domain.Movies;
using Movies.Infrastructure.Context;

namespace Movies.Infrastructure.Data.Repositories;

public class MovieRepository(MoviesContext context) : IMovieRepository
{
    public IEnumerable<Movie> GetMovies()
    {
        return context.Movies.ToList();
    }
}
