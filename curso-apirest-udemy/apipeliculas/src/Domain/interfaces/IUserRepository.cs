using apipeliculas.src.auth;
using apipeliculas.src.auth.Dtos;
using apipeliculas.src.Domain.Models;

namespace apipeliculas.src.Domain.interfaces
{
    public interface IUserRepository
    {
        ICollection<User> FindAll();
        User FindById(int id);
        bool IsUniqueUser(string username);
        Task<UserLoginResponseDTO> Login(UserLoginDTO dto);
        Task<User> Register(UserRegisterDTO dto);
    }
}