using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string MovieLength { get; set; }
        //[DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        //[DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int? TotalCost { get; set; }
        public int? TotalRevenue { get; set; }
        public string? Poster { get; set; }
        public string Trailer { get; set; }
        public string MovieState { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        public List<Showtime?> Showtimes { get; set; }
        //public ICollection<Showtime> Showtimes { get; set; }
        public Movie()
        {
            // Initialize Showtimes property as an empty list
            Showtimes = new List<Showtime>();
        }
    }
}
