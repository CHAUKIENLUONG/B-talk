using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class InventoryEntry
    {
        [Key]
        public int InventoryEntryId { get; set; }

        public int ComboId { get; set; }
        public Combo Combo { get; set; }  // Điều này giúp dễ dàng truy cập thông tin Combo

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Entry Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EntryDate { get; set; } // Thời gian nhập hàng
    }

}
