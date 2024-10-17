using System.ComponentModel.DataAnnotations;
using WebCinema.Models;

namespace WebCinema.ViewModels
{
    public class BookingSeatViewModel
    {
        public Movie Movie { get; set; }
        public Showtime Showtime { get; set; }
        public Screentime Screentime { get; set; }
        public decimal TotalAmount { get; set; }
        public List<string> BookedSeats { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public string VoucherCode { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
