using WebCinema.Models;

namespace WebCinema.Repositories
{
    public interface IShowtimeRepo
    {
        Task<List<Showtime>> GetByMovieIdAsync(int movieId);

        Task<Showtime> GetByIdAsync(int id);

        IEnumerable<Showtime> GetShowtimesForDate(DateTime selectedDate);
        Task AddAsync(Showtime showtime);
        Task<bool> IsShowtimeDuplicateAsync(int movieId, string roomId, DateTime showDate, int screenTimeId);
    }
}
