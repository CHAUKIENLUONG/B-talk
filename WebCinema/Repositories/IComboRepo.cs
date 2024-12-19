using WebCinema.Models;

namespace WebCinema.Repositories
{
    // IComboRepository.cs
    public interface IComboRepo
    {
        Task<List<Combo>> GetAllCombosAsync(); // Lấy tất cả các combo
        Task<Combo> GetComboByIdAsync(int comboId); // Lấy combo theo ID
        Task AddComboAsync(Combo combo); // Thêm combo
        Task UpdateComboAsync(Combo combo); // Cập nhật combo
        Task DeleteComboAsync(int comboId); // Xóa combo
    }
}
