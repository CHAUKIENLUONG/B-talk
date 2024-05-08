using WebCinema.Models;

namespace WebCinema.Repositories
{
    public interface IMovieRepo
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<Movie> GetByIdAsync(int id);
        Task<Showtime> GetByMovieIdAsync(int id);

        Task AddAsync(Movie movie);
        Task AddShowtimeAsync(Showtime showtime);
        Task UpdateAsync(Movie movie);
        Task DeleteAsync(int id);
        Task<IEnumerable<Movie>> GetAllWithShowtimesAndScreentimesAsync();
        //Task<IEnumerable<Movie>> GetSelectedDateFromDatabase();
        Task<IEnumerable<Movie>> GetAllShowAsync(int movieId);
        Task<Movie> GetShowByMovieIdAsync(int id);
        Task<Movie> GetMovieByNameAsync(string movieName);
    }
}
