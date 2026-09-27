namespace Movies.Domain.Movies;

public interface IMovieRepository
{
    IEnumerable<Movie> GetMovies();
}