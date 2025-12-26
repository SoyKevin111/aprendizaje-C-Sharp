using Microsoft.EntityFrameworkCore;
using projectef.src.models;

namespace projectef;

public class TasksContext : DbContext
{
  public DbSet<Category> Category { get; set; }
  public DbSet<TodoTask> Task { get; set; }
  public TasksContext(DbContextOptions<TasksContext> options) : base(options) { }
}