using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class TicketCombo
    {
        [Key]
        public int TicketComboId { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public int ComboId { get; set; }
        public Combo Combo { get; set; }
    }
}
