using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class Combo
    {
        [Key]
        public int ComboId { get; set; }
        public string ComboName { get; set; }
        public int? ComboPrice { get; set; }

        public ICollection<TicketCombo> TicketCombos { get; set; }
    }
}
