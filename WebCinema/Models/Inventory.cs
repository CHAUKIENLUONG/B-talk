using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCinema.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public int ComboId { get; set; }
        public Combo Combo { get; set; }

        public int QuantityInStock { get; set; }
        public int QuantitySold { get; set; }

        // Tính toán giá trị RemainingQuantity mỗi khi cập nhật
        [NotMapped]
        public int RemainingQuantity => QuantityInStock - (QuantitySold * 500);
    }

}
