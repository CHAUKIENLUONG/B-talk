using WebCinema.Models;

namespace WebCinema.Repositories
{
    public interface IRoomRepo
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room> GetByIdAsync(string id);
    }
}
