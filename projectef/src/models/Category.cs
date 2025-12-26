using System.ComponentModel.DataAnnotations;

namespace projectef.src.models;

public class Category
{
  public Guid CategoryId { get; set; }
  public string Name { get; set; }

  public string Descripcion { get; set; }
  public virtual ICollection<TodoTask> Tasks { get; set; } //proxies, lazy loading
}