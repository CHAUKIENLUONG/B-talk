using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class TicketInfo
    {
        [Key]
        public int TicketInfoId { get; set; }
        public bool? State { get; set; }

        public int? TicketID { get; set; }
        public string SeatId { get; set; }

        public Seat Seat { get; set; }
        public Ticket Ticket { get; set; }
    }
}
