using apipeliculas.src.auth;
using apipeliculas.src.auth.Dtos;
using apipeliculas.src.Domain.Models;

namespace apipeliculas.src.Domain.interfaces
{
    public interface IUserRepository
    {
        Task<ICollection<AppUser>> FindAll();
        Task<AppUser> FindById(string id);
        Task<bool> IsUniqueUser(string username);
        Task<UserLoginResponseDTO> Login(UserLoginDTO dto);
        Task<UserDataDTO> Register(UserRegisterDTO dto);
    }
}
