namespace WebCinema.Models;
public class Branch
{
    public int BranchId { get; set; } // Khóa chính
    public string BranchName { get; set; } // Tên chi nhánh
    // Thêm các thuộc tính khác nếu cần

    public ICollection<Movie>? Movies { get; set; }
}