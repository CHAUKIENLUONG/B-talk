using Microsoft.EntityFrameworkCore;
using WebCinema.Models;

namespace WebCinema.Repositories
{
    public class EFRoomRepo : IRoomRepo
    {

        private readonly ApplicationDbContext _context;
        public EFRoomRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }
        public async Task<Room> GetByIdAsync(string id)
        {
            return await _context.Rooms.FindAsync(id);
        }
    }
}
