using System.ComponentModel.DataAnnotations;

namespace apipeliculas.src.auth
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Username obligatorio.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password obligatorio.")]
        public string Password { get; set; }
    }
}