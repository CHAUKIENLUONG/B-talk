using WebCinema.Models;

namespace WebCinema.Repositories
{
    public interface IMovieDetailRepo
    {
        Task<Movie> GetMovieByIdAsync(int movieId);

    }
}
