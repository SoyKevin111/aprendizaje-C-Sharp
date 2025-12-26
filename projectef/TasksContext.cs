using Microsoft.EntityFrameworkCore;
using projectef.src.models;

namespace projectef;

public class TasksContext : DbContext
{
  public DbSet<Category> Categories { get; set; }
  public DbSet<TodoTask> Tasks { get; set; }
  public TasksContext(DbContextOptions<TasksContext> options) : base(options) { }
}