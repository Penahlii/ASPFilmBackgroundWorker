using FilmBackgroundWorker.Entities;
using FilmBackgroundWorker.Services;
using FilmBackgroundWorker.Settings;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using FilmBackgroundWorker.Data;

public class FilmMachineBackgroundWorker : BackgroundService
{
    private readonly OmdbApiService _omdbApiService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly float _delayMinutes;

    public FilmMachineBackgroundWorker(
        OmdbApiService omdbApiService,
        IServiceScopeFactory scopeFactory,
        IOptions<BackgroundWorkerSettings> settings)
    {
        _omdbApiService = omdbApiService;
        _scopeFactory = scopeFactory;
        _delayMinutes = settings.Value.DelayMinutes;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var random = new Random();

        while (!stoppingToken.IsCancellationRequested)
        {
            char randomLetter = (char)random.Next('A', 'Z' + 1);
            Console.WriteLine($"Fetching films starting with: {randomLetter}");

            var response = await _omdbApiService.SearchFilmsAsync(randomLetter.ToString());

            if (response["Response"].ToString() == "True")
            {
                var films = response["Search"].Select(f => f["Title"].ToString()).ToList();

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDataContext>();

                    foreach (var filmTitle in films)
                    {
                        var existingFilm = await dbContext.Films
                            .Where(f => f.Title == filmTitle)
                            .FirstOrDefaultAsync(stoppingToken);

                        if (existingFilm == null)
                        {
                            var newFilm = new Film
                            {
                                Title = filmTitle,
                                Genre = randomLetter.ToString() // do not need a specific Genre (just random)
                            };
                            dbContext.Films.Add(newFilm);
                            await dbContext.SaveChangesAsync(stoppingToken);
                            Console.WriteLine($"Added new film: {filmTitle}");
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No films found for the letter.");
            }

            await Task.Delay(TimeSpan.FromMinutes(_delayMinutes), stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Background task is stopping...");
        return base.StopAsync(cancellationToken);
    }
}
