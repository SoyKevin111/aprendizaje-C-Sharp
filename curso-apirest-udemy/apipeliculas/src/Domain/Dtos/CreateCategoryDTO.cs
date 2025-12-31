using System.ComponentModel.DataAnnotations;

namespace apipeliculas.src.Dtos
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "Caracteres maximos: 100!")]
        public string Name { get; set; }
    }
}