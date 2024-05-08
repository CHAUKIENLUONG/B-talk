using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCinema.Models;
using WebCinema.Repositories;

namespace WebCinema.Controllers
{
    public class ShowTimesController : Controller
    {
        
        private readonly IGenreRepo _genreRepo;
        private readonly IMovieRepo _movieRepo;
        private readonly IShowtimeRepo _showtimeRepo;
        private readonly ApplicationDbContext _context;
        
        public ShowTimesController(IMovieRepo movieRepo, IGenreRepo genreRepo, 
            ApplicationDbContext context, IShowtimeRepo showtimeRepo)
        {
            _movieRepo = movieRepo;
            _genreRepo = genreRepo;
            _showtimeRepo = showtimeRepo;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetShowtimes(DateTime selectedDate)
        {
            // Retrieve showtimes from the database based on the selected date
            var showtimes = _context.Showtimes.Where(s => s.ShowtimeDate.Date == selectedDate.Date)
            .ToList(); 

            // Return a partial view with the updated showtimes
            return PartialView("_ShowtimesPartial", showtimes);
        }

        public async Task<IActionResult> ShowTimes()
        {
            DateTime selectedDate = DateTime.Today;
            if (Request.Query.ContainsKey("selectedDate"))
            {
                DateTime.TryParse(Request.Query["selectedDate"], out selectedDate);
            }
            var movies = await _movieRepo.GetAllAsync();
            // Use selectedDate if needed for further processing

            ViewBag.SelectedDate = selectedDate; // Optional: Set ViewBag here
            var moviesWithShowtimes = await _movieRepo.GetAllWithShowtimesAndScreentimesAsync();
            return View(moviesWithShowtimes);
        }

        [HttpPost]
        public IActionResult SelectShowtime(int movieId, int showtimeId)
        {
            return RedirectToAction("BookingSeat", "Ticket", new { movieId = movieId, showtimeId = showtimeId });
        }
        

        //public async Task<IActionResult> Index()
        //{
        //    var showtimes = await _showtimeRepo.GetAllAsync();
        //    if (showtimes == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(showtimes);
        //}

        //public async Task<IActionResult> Display(int id)
        //{
        //    var category = await _categoryRepository.GetByIdAsync(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(category);
        //}

        //public async Task<IActionResult> Add(int movieId)
        //{
        //    var showtime = await _showtimeRepo.GetByMovieId(movieId);
        //    if (showtime == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(showtime);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Add(int movieId, Showtime showtime)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _showtimeRepo.AddAsync(movieId,showtime);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(showtime);
        //}

        //public async Task<IActionResult> Update(int id)
        //{
        //    var showtime = await _showtimeRepo.GetByIdAsync(id);
        //    if (showtime == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(showtime);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(int id, Showtime showtime)
        //{
        //    if (id != showtime.ShowId)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        await _showtimeRepo.UpdateAsync(showtime);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(showtime);
        //}

        ////public async Task<IActionResult> Delete(int id)
        ////{
        ////    var category = await _categoryRepository.GetByIdAsync(id);
        ////    if (category == null)
        ////    {
        ////        return NotFound();
        ////    }
        ////    return View(category);
        ////}
        //[HttpPost, ActionName("DeleteConfirmed")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var category = await _categoryRepository.GetByIdAsync(id);
        //    if (category != null)
        //    {
        //        await _categoryRepository.DeleteAsync(id);
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
    }

}
