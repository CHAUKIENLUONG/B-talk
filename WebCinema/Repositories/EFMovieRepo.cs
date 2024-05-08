using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebCinema.Controllers;
using WebCinema.Models;

namespace WebCinema.Repositories
{

    public class EFMovieRepo : IMovieRepo
    {

        private readonly ApplicationDbContext _context;
        public EFMovieRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            // return await _context.Movies.ToListAsync();
            return await _context.Movies
            .Include(p => p.Genre) // Include thông tin về category
            .ToListAsync();
        }
        public async Task<Movie> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Movies.Include(p => p.Genre).FirstOrDefaultAsync(p => p.MovieId == id);
        }
        public async Task<Showtime> GetByMovieIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Showtimes
                .FirstOrDefaultAsync(p => p.MovieId == id);

        }
        public async Task AddAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }
        public async Task AddShowtimeAsync(Showtime showtime)
        {
            _context.Showtimes.Add(showtime);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllWithShowtimesAndScreentimesAsync()
        {
            return await _context.Movies
                .Include(m => m.Showtimes)
                    .ThenInclude(s => s.Screentime)
                .ToListAsync();
        }
        //public async Task<IEnumerable<Movie>> GetSelectedDateFromDatabase()
        //{
        //    return await _context.Movies
        //        .Include(m => m.da)
        //            .ThenInclude(s => s.Screentime)
        //        .ToListAsync();
        //}
        public async Task<IEnumerable<Movie>> GetAllShowAsync(int movieId)
        {
            return await _context.Movies.Where(x => x.MovieId == movieId).ToListAsync();
        }
        public async Task<Movie> GetShowByMovieIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie> GetMovieByNameAsync(string movieName)
        {
            return await _context.Movies.FirstOrDefaultAsync(m => m.MovieName == movieName);
        }
    }
}
