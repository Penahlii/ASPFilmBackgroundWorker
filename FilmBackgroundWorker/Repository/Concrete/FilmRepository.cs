using FilmBackgroundWorker.Data;
using Microsoft.EntityFrameworkCore;
using FilmBackgroundWorker.Entities;
using FilmBackgroundWorker.Repository.Abstract;

namespace FilmBackgroundWorker.Repository.Concrete;

public class FilmRepository : IFilmRepository
{
    private readonly AppDataContext _context;

    public FilmRepository(AppDataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Film>> GetAllAsync()
    {
        return await _context.Films.ToListAsync();
    }

    public async Task<Film> GetByIdAsync(int id)
    {
        return await _context.Films.FindAsync(id);
    }

    public async Task AddAsync(Film film)
    {
        await _context.Films.AddAsync(film);
    }

    public async Task UpdateAsync(Film film)
    {
        _context.Films.Update(film);
    }

    public async Task DeleteAsync(Film film)
    {
        _context.Films.Remove(film);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Film?> GetFilmByTitleAsync(string title)
    {
        return await _context.Films.FirstOrDefaultAsync(f => f.Title == title);
    }
}
