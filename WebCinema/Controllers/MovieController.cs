using Microsoft.AspNetCore.Mvc;
using WebCinema.Repositories;

namespace WebCinema.Controllers
{

    public class MovieController : Controller
    {
        private readonly IGenreRepo _genreRepo;
        private readonly IMovieRepo _movieRepo;

        public MovieController(IMovieRepo movieRepo, IGenreRepo genreRepo)
        {
            _movieRepo = movieRepo;
            _genreRepo = genreRepo;
        }
        public async Task<IActionResult> Movie()
        {
            var movies = await _movieRepo.GetAllAsync();
            return View(movies);
        }



    }

}
