using Microsoft.AspNetCore.Identity;

namespace apipeliculas.src.Domain.Models
{
    public class AppUser : IdentityUser
    {
        //Datos con campos adicionales a la tabla.
        public string Name { get; set; }
        public string CreatedAt { get; set; }
    }
}