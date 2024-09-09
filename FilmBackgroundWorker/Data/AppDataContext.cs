using FilmBackgroundWorker.Entities;
using Microsoft.EntityFrameworkCore;

namespace FilmBackgroundWorker.Data;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
    {
    }

    public virtual DbSet<Film> Films { get; set; }
}
