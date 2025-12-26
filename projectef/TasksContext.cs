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

    List<Category> categoriesInit =
    [
      new Category()
      {
        CategoryId = Guid.Parse("baed9063-a68c-4e94-8f55-19c2fc68f75b"),
        Name = "Actividades Pendientes",
        Descripcion = "lorem ipsun door af terque cua fofvy ether who",
        Peso = int.Parse("60")
      },
      new Category()
      {
        CategoryId = Guid.Parse("baed9063-a68c-4e94-8f55-19c2fc68f752"),
        Name = "Actividades Personales",
        Descripcion = "lorem ipsun door af terque cua fofvy ether who",
        Peso = int.Parse("70")
      },
    ];

    List<TodoTask> tasksInit =
    [
      new TodoTask()
      {
        TaskId = Guid.Parse("baed9063-a68c-4e94-8f55-19c2fc68f751"),
        CategoryId = Guid.Parse("baed9063-a68c-4e94-8f55-19c2fc68f75b"),
        PriorityTask = Priority.Medium,
        Title = "Pago de servicios Publicos",
        Create_on = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
      },
      new TodoTask()
      {
        TaskId = Guid.Parse("baed9063-a68c-4e94-8f55-19c2fc68f759"),
        CategoryId = Guid.Parse("baed9063-a68c-4e94-8f55-19c2fc68f752"),
        PriorityTask = Priority.high,
        Title = "Pago de servicios Privados",
        Create_on = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
      }
    ];

    modelBuilder.Entity<Category>(category =>
    {
      category.ToTable("category");
      category.HasKey(c => c.CategoryId);
      category.Property(c => c.Name).IsRequired().HasMaxLength(150);
      category.Property(c => c.Descripcion).IsRequired(false);
      category.Property(c => c.Peso);
      category.Property(c => c.Tag).IsRequired(false); ;

      category.HasData(categoriesInit);
    });

    modelBuilder.Entity<TodoTask>(task =>
    {
      task.ToTable("task");
      task.HasKey(t => t.TaskId);
      //t.category | c.tasks -> foreignKey 
      task.HasOne(t => t.category).WithMany(c => c.Tasks).HasForeignKey(t => t.CategoryId);//categoryId
      task.Property(t => t.Title).IsRequired().HasMaxLength(200);
      task.Property(t => t.Description).IsRequired(false);
      task.Property(t => t.PriorityTask);
      task.Property(t => t.Create_on);
      task.Ignore(t => t.Summary); //no mapear

      task.HasData(tasksInit);

    });
  }
}