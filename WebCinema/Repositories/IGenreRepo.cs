using WebCinema.Models;

namespace WebCinema.Repositories
{
    public interface IGenreRepo
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int id);
        Task AddAsync(Genre genre);
        Task UpdateAsync(Genre genre);
        Task DeleteAsync(int id);

    }
}
