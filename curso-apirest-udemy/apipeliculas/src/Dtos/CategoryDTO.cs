using System.ComponentModel.DataAnnotations;

namespace apipeliculas.src.Dtos
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "Caracteres maximos: 100!")]
        public string Name { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}