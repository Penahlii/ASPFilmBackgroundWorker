using FilmBackgroundWorker.Data;
using FilmBackgroundWorker.Entities;
using FilmBackgroundWorker.Repository.Abstract;
using FilmBackgroundWorker.Repository.Concrete;
using FilmBackgroundWorker.Services;
using FilmBackgroundWorker.Settings;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<OmdbApiService>();
builder.Services.AddScoped<IFilmRepository, FilmRepository>();
builder.Services.Configure<BackgroundWorkerSettings>(
    builder.Configuration.GetSection("BackgroundWorkerSettings"));


builder.Services.AddHostedService<FilmMachineBackgroundWorker>();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDataContext>(opt =>
{
    opt.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
