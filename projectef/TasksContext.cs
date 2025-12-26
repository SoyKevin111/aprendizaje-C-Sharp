using Microsoft.EntityFrameworkCore;
using projectef.src.models;

namespace projectef;

public class TasksContext : DbContext
{
  public DbSet<Category> Category { get; set; }
  public DbSet<TodoTask> Task { get; set; }
  public TasksContext(DbContextOptions<TasksContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Category>(category =>
    {
      category.ToTable("Category");
      category.HasKey(c => c.CategoryId);
      category.Property(c => c.Name).IsRequired().HasMaxLength(150);
      category.Property(c => c.Descripcion);
    });

    modelBuilder.Entity<TodoTask>(task =>
    {
      task.ToTable("task");
      task.HasKey(t => t.TaskId);
      //t.category | c.tasks -> foreignKey 
      task.HasOne(t => t.category).WithMany(c => c.Tasks).HasForeignKey(t => t.CategoryId);//categoryId
      task.Property(t => t.Title).IsRequired().HasMaxLength(200);
      task.Property(t => t.Description);
      task.Property(t => t.PriorityTask);
      task.Property(t => t.Create_on);
      task.Ignore(t => t.Summary); //no mapear

    });
  }
}