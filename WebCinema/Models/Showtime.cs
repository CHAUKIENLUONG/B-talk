using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace WebCinema.Models
{
    public class Showtime
    {
        [Key]
        public int ShowId { get; set; }
        public string RoomId { get; set; }
        public DateTime ShowtimeDate { get; set; }
        public int ScreenTimeId { get; set; }
        public int? SoldTickets { get; set; }
        public int? TotalRevenue { get; set; }

        public int MovieId { get; set; }

        public Movie? Movie { get; set; }
        public Room? Room { get; set; }
        public Screentime? Screentime { get; set; }
        //public SelectList Screentimes { get; set; }


        public ICollection<Ticket>? Tickets { get; set; }
    }
}
