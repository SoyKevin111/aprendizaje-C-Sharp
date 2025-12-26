using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectef.src.models;


public class TodoTask
{
  [Key]// Main Key
  public Guid TaskId { get; set; }

  [ForeignKey("CategoryId")]
  public Guid CategoryId { get; set; }

  [Required]
  [MaxLength(200)]
  public string Title { get; set; }
  public string Description { get; set; }
  public Priority PriorityTask { get; set; }
  public DateTime Create_on { get; set; }
  public virtual Category category { get; set; }

  [NotMapped] //omite mapeo de este campo
  public string Summary { get; set; }
}


public enum Priority
{
  Low,
  Medium,
  high
}