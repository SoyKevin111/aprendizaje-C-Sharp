using apipeliculas.src.Domain.Models;

namespace apipeliculas.src.auth.Dtos
{
    public class UserLoginResponseDTO
    {
        public User User { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}