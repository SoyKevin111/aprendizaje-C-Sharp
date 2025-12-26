using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectef.src.models;


public class TodoTask
{
  public Guid TaskId { get; set; }
  public Guid CategoryId { get; set; }
  public string Title { get; set; }
  public string Description { get; set; }
  public Priority PriorityTask { get; set; }
  public DateTime Create_on { get; set; }
  public virtual Category category { get; set; } //cree proxies, lazy loading
  public string Summary { get; set; }
}


public enum Priority
{
  Low,
  Medium,
  high
}