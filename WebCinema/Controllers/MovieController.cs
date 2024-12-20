using Microsoft.AspNetCore.Mvc;
using WebCinema.Repositories;
using WebCinema.Models;
using Microsoft.EntityFrameworkCore;

namespace WebCinema.Controllers
{

    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Movie()
        {
            var today = DateTime.Today;
            
            // Lấy tất cả phim và cập nhật trạng thái
            var movies = await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Branch)
                .ToListAsync();

            // Cập nhật trạng thái phim dựa trên ngày
            foreach (var movie in movies)
            {
                // Nếu ngày chiếu là quá khứ (trước hôm nay), không hiển thị
                if (movie.ReleaseDate.Date < today)
                {
                    movie.MovieState = "0"; // Đã kết thúc
                }
                // Nếu ngày chiếu là hôm nay
                else if (movie.ReleaseDate.Date == today)
                {
                    movie.MovieState = "1"; // Đang chiếu
                }
                // Nếu ngày chiếu là tương lai (sau hôm nay)
                else
                {
                    movie.MovieState = "2"; // Sắp chiếu
                }

                // Cập nhật vào database
                _context.Update(movie);
            }
            await _context.SaveChangesAsync();

            // Chỉ lấy phim đang chiếu và sắp chiếu
            movies = movies.Where(m => m.MovieState != "0").ToList();

            ViewBag.Branches = await _context.Branches.ToListAsync();
            return View(movies);
        }

        // API endpoint để lấy phim theo chi nhánh
        [HttpGet]
        public async Task<IActionResult> GetMoviesByBranch(int branchId)
        {
            var movies = await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Branch)
                .Where(m => branchId == 0 || m.BranchId == branchId)
                .ToListAsync();

            return Json(movies);
        }



    }

}
