using WebCinema.Models;

namespace WebCinema.Repositories
{
    public interface IScreentimeRepo
    {
        Task<IEnumerable<Screentime>> GetAllAsync();
        Task<Screentime> GetByIdAsync(int id);
        Task<Screentime> GetScreentimeByTimeAsync(TimeSpan screenTime);
    }
}
