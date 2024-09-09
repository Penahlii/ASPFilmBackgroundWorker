using FilmBackgroundWorker.Entities;

namespace FilmBackgroundWorker.Repository.Abstract;

public interface IFilmRepository
{
    Task<IEnumerable<Film>> GetAllAsync();
    Task<Film> GetByIdAsync(int id);
    Task AddAsync(Film film);
    Task UpdateAsync(Film film);
    Task DeleteAsync(Film film);
    Task SaveAsync();
    Task<Film?> GetFilmByTitleAsync(string title);
}
