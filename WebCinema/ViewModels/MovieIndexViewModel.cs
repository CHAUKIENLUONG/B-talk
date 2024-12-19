using WebCinema.Models;

namespace WebCinema.ViewModels
{
    public class MovieIndexViewModel
    {
        public IEnumerable<WebCinema.Models.Movie> Movies { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
    }
}
