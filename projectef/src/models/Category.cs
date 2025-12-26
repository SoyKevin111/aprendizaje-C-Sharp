using System.ComponentModel.DataAnnotations;

namespace projectef.src.models;

public class Category
{
  [Key]// Main Key
  public Guid CategoryId { get; set; }

  [Required]
  [MaxLength(150)]
  public string Name { get; set; }

  public string Descripcion { get; set; }
  public virtual ICollection<TodoTask> Tasks { get; set; }
}