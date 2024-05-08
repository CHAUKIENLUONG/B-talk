using Microsoft.EntityFrameworkCore;
using WebCinema.Controllers;
using WebCinema.Models;

namespace WebCinema.Repositories
{
    public class EFMovieDetailRepo : IMovieDetailRepo
    {
        private readonly ApplicationDbContext _context;

        public EFMovieDetailRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> GetMovieByIdAsync(int movieId)
        {
            return await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == movieId);
        }
    }
}
