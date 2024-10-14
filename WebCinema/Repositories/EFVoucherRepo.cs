using Microsoft.EntityFrameworkCore;
using WebCinema.Models;

namespace WebCinema.Repositories
{
    public class EFVoucherRepo : IVoucherRepo
    {
        private readonly ApplicationDbContext _context;

        public EFVoucherRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Voucher voucher)
        {
            _context.Vouchers.AddAsync(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Voucher>> GetAllAsync()
        {
            return await _context.Vouchers.ToListAsync();
        }

        public async Task<Voucher> GetByIdAsync(int id)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
        }
    }
}
