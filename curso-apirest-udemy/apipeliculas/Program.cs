using apipeliculas.src.Data;
using apipeliculas.src.Mapper;
using apipeliculas.src.Repositories;
using apipeliculas.src.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AplicationDbContext>(opciones =>
    opciones.UseNpgsql(builder.Configuration.GetConnectionString("cnnPostgres"))
);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

// Add repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

// Add AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(MoviesMapper));

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(() =>
{
    logger.LogInformation("Swagger UI disponible en: http://localhost:5000/swagger");
});

app.Run();
