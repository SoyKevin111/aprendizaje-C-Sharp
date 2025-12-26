using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectef;
using projectef.src.models;


var builder = WebApplication.CreateBuilder(args);

//obtener configuracion de la conextionStrings;
var connectionString = builder.Configuration.GetConnectionString("cnPostgres");

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

app.MapGet("/api/tasks", async ([FromServices] TasksContext dbContext) =>
{
  return Results.Ok(dbContext.Task.Include(t => t.category));
});

app.MapPost("/api/tasks", async ([FromServices] TasksContext dbContext, [FromBody] TodoTask task) =>
{
  task.TaskId = Guid.NewGuid();
  task.Create_on = DateTime.UtcNow; //zona horaria local global
  await dbContext.Task.AddAsync(task);
  await dbContext.SaveChangesAsync();
  return Results.Ok();
});

app.MapPut("/api/tasks/{id}", async ([FromServices] TasksContext dbContext, [FromBody] TodoTask task, [FromRoute] Guid id) =>
{

  //buscar tarea actual
  var currentTask = dbContext.Task.Find(id);
  if (currentTask == null)
  {
    return Results.NotFound();
  }
  currentTask.CategoryId = task.CategoryId;
  currentTask.Title = task.Title;
  currentTask.PriorityTask = task.PriorityTask;
  currentTask.Description = task.Description;

  await dbContext.SaveChangesAsync();
  return Results.Ok();
});

app.MapDelete("/api/tasks/{id}", async ([FromServices] TasksContext dbContext, [FromRoute] Guid id) =>
{
  var currentTask = dbContext.Task.Find(id);
  if (currentTask != null)
  {
    dbContext.Remove(currentTask);
    await dbContext.SaveChangesAsync();
    return Results.Ok();
  }
  return Results.NotFound();
});


//probar: http://localhost:5167/dbconnection

app.Run();
