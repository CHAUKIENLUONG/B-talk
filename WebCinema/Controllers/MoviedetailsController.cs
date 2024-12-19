using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCinema.Models;
using WebCinema.Repositories;

namespace WebCinema.Controllers
{

    public class MoviedetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenreRepo _genreRepo;
        private readonly IMovieRepo _movieRepo;

        public MoviedetailsController(IMovieRepo movieRepo, IGenreRepo genreRepo, ApplicationDbContext context)
        {
            _context = context;
            _movieRepo = movieRepo;
            _genreRepo = genreRepo;
        }
        public async Task<IActionResult> MovieDetails(int? movieId)
        {

            if (movieId == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.Include(m => m.Genre)
                                             .FirstOrDefaultAsync(m => m.MovieId == movieId);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

    }
}
