﻿using Microsoft.EntityFrameworkCore;
using WebCinema.Models;

namespace WebCinema.Repositories
{
    public class EFGenreRepo : IGenreRepo
    {
        private readonly ApplicationDbContext _context;
        public EFGenreRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }
        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }
        public async Task AddAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Genre genre)
        {
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
    }
}
