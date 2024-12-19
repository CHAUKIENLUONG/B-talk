using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebCinema.Models;

namespace WebCinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var topMovies = _context.Movies
                .Include(m => m.Genre)
                .Join(
                    _context.Tickets,
                    movie => movie.MovieName,
                    ticket => ticket.MovieName,
                    (movie, ticket) => new { Movie = movie, Ticket = ticket }
                )
                .GroupBy(x => new { x.Movie.MovieId, x.Movie.MovieName, x.Movie.Poster })
                .Select(g => new {
                    Movie = new { 
                        MovieId = g.Key.MovieId,
                        MovieName = g.Key.MovieName,
                        Poster = g.Key.Poster
                    },
                    TicketCount = g.Count()
                })
                .OrderByDescending(x => x.TicketCount)
                .Take(3)
                .ToList();

            ViewBag.TopMovies = topMovies;
            var movies = _context.Movies.Include(m => m.Genre).ToList();
            return View(movies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

      
    }
}
