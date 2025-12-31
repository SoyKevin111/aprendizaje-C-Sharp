using System.ComponentModel.DataAnnotations;

namespace apipeliculas.src.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public Movie MovieData { get; set; }
    }
}