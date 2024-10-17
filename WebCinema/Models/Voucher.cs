using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class Voucher
    {
        [Key]
        public int Id { get; set; } // Mã ưu đãi
        public string Code { get; set; } // Mã ưu đãi
        public string Description { get; set; } // Miêu tả
        public DateTime ReleaseDate { get; set; } // Ngày phát hành
        public DateTime EndDate { get; set; } // Ngày hết hạn
        public decimal DiscountAmount { get; set; }
        public bool IsActive { get; set; }

        public Voucher()
        {
            IsActive = true; // Khởi tạo voucher là hoạt động
        }
    }
}
