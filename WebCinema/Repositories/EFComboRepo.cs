using WebCinema.Models;
using Microsoft.EntityFrameworkCore;

namespace WebCinema.Repositories
{
    // ComboRepository.cs
    public class EFComboRepo : IComboRepo
    {
        private readonly ApplicationDbContext _context;

        public EFComboRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các combo
        public async Task<List<Combo>> GetAllCombosAsync()
        {
            return await _context.Combos.ToListAsync();
        }

        // Lấy combo theo ID
        public async Task<Combo> GetComboByIdAsync(int comboId)
        {
            return await _context.Combos.FindAsync(comboId);
        }

        // Thêm combo mới
        public async Task AddComboAsync(Combo combo)
        {
            await _context.Combos.AddAsync(combo);
            await _context.SaveChangesAsync();
        }

        // Cập nhật combo
        public async Task UpdateComboAsync(Combo combo)
        {
            _context.Combos.Update(combo);
            await _context.SaveChangesAsync();
        }

        // Xóa combo
        public async Task DeleteComboAsync(int comboId)
        {
            var combo = await _context.Combos.FindAsync(comboId);
            if (combo != null)
            {
                _context.Combos.Remove(combo);
                await _context.SaveChangesAsync();
            }
        }
    }

}
