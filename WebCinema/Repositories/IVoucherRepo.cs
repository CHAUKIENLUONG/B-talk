using System.Collections.Generic;
using System.Threading.Tasks;
using WebCinema.Models;

namespace WebCinema.Repositories
{
    public interface IVoucherRepo
    {
        Task<IEnumerable<Voucher>> GetAllAsync();
        Task<Voucher> GetByIdAsync(int id);
        Task<Voucher> GetByCodeAsync(string code);
        Task AddAsync(Voucher voucher);
        Task UpdateAsync(Voucher voucher);
        Task DeleteAsync(int id);
    }
}
