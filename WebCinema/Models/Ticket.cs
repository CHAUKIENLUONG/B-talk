using static Azure.Core.HttpHeader;
using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public string MovieName { get; set; }
        public string RoomName { get; set; }
        public TimeSpan ScreenTime { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ShowDate { get; set; }
        public int Total { get; set; }

        //public int? ComboId { get; set; }
        public int ShowId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        //public string UserId { get; set; }
        //public ApplicationUser User { get; set; } // Navigation property

        public Showtime Showtime { get; set; }
        //public Combo Combo { get; set; }

        public ICollection<TicketCombo>? TicketCombos { get; set; }
        public ICollection<TicketInfo> TicketInfos { get; set; }
    }
}
