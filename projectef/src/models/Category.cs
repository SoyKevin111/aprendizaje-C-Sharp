using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace projectef.src.models;

public class Category
{
  public Guid CategoryId { get; set; }
  public string Name { get; set; }
  public string Descripcion { get; set; }
  public int Peso { get; set; }
  public string Tag { get; set; }
  [JsonIgnore]
  public virtual ICollection<TodoTask> Tasks { get; set; } //proxies, lazy loading
}