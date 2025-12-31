using apipeliculas.src.Models;

namespace apipeliculas.src.Dtos
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string ImageUrl { get; set; }
        public TypeClasification Clasification { get; set; }
        public DateTime CreatedAt { get; set; }

        //Relacion categoria
        public int CategoryId { get; set; }
    }
}