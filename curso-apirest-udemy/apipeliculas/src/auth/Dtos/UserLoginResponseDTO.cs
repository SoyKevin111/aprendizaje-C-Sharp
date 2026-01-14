using apipeliculas.src.Domain.Models;

namespace apipeliculas.src.auth.Dtos
{
    public class UserLoginResponseDTO
    {
        public UserDataDTO User { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}