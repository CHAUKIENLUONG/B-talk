using System.ComponentModel.DataAnnotations;

namespace WebCinema.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        public string GenreName { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
