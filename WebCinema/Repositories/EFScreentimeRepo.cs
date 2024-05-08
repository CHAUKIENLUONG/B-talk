using Microsoft.EntityFrameworkCore;
using WebCinema.Models;

namespace WebCinema.Repositories
{
    public class EFScreentimeRepo : IScreentimeRepo
    {
        private readonly ApplicationDbContext _context;
        public EFScreentimeRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Screentime>> GetAllAsync()
        {
            return await _context.Screentimes.ToListAsync();
        }
        public async Task<Screentime> GetByIdAsync(int id)
        {
            return await _context.Screentimes.FindAsync(id);
        }

        public async Task<Screentime> GetScreentimeByTimeAsync(TimeSpan screenTime)
        {
            return await _context.Screentimes.FirstOrDefaultAsync(s => s.ScreenTime == screenTime);
        }
    }
}
