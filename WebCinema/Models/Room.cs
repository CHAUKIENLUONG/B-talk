using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class Room
    {
        [Key]
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public int? TotalSeat { get; set; }

        public ICollection<Seat> Seats { get; set; }
        public ICollection<Showtime> Showtimes { get; set; }
    }
}
