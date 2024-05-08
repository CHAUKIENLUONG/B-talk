using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class Seat
    {
        [Key]
        public string SeatId { get; set; }
        public string RoomId { get; set; }

        public Room Room { get; set; }

        public ICollection<TicketInfo> Ticketinfos { get; set; }
    }
}
