using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectef;

var builder = WebApplication.CreateBuilder(args);

//configurar Entity framework
builder.Services.AddDbContext<TasksContext>(p => p.UseInMemoryDatabase("TasksDB"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/dbconnection", async ([FromServices] TasksContext dbContext) =>
{
  dbContext.Database.EnsureCreated();
  return Results.Ok("DB in memory : " + dbContext.Database.IsInMemory());
});

//probar: http://localhost:5167/dbconnection

app.Run();
