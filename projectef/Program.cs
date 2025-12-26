using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectef;

var builder = WebApplication.CreateBuilder(args);

//obtener configuracion de la conextionStrings;
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

//Registrar DbContext con Postgresql
builder.Services.AddDbContext<TasksContext>(options =>
  options.UseNpgsql(connectionString)
);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/dbconnection", async ([FromServices] TasksContext dbContext) =>
{
  try
  { //se crea al entrar a una peticion http
    dbContext.Database.EnsureCreated();
    bool canConnect = await dbContext.Database.CanConnectAsync();
    return Results.Ok("Conexion is: " + canConnect);
  }
  catch (Exception ex)
  {
    return Results.Problem(ex.Message);
  }
});

//probar: http://localhost:5167/dbconnection

app.Run();
