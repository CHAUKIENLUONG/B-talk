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

        // Thêm trường lưu trữ đường dẫn hoặc URL của hình ảnh

    }
}
