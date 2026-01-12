using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using apipeliculas.src.auth;
using apipeliculas.src.auth.Dtos;
using apipeliculas.src.Data;
using apipeliculas.src.Domain.interfaces;
using apipeliculas.src.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace apipeliculas.src.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AplicationDbContext _db;
        private readonly string _SECRET_KEY;

        public UserRepository(AplicationDbContext db, IConfiguration cfg)
        {
            this._db = db;
            this._SECRET_KEY = cfg.GetValue<string>("ApiSettings:Secret_Key");
        }

        public ICollection<User> FindAll()
        {
            return _db.User.OrderBy(u => u.Username).ToList();
        }

        public User FindById(int id)
        {
            return _db.User.FirstOrDefault(u => u.Id == id);
        }

        public bool IsUniqueUser(string username)
        {
            var userDB = _db.User.FirstOrDefault(u => u.Username == username);
            return userDB == null;
        }

        public async Task<UserLoginResponseDTO> Login(UserLoginDTO dto)
        {
            var passwordEncrypt = GetMD5(dto.Password);
            var user = _db.User.FirstOrDefault(
                u => u.Username.Equals(dto.Username, StringComparison.CurrentCultureIgnoreCase)
                && u.Password == passwordEncrypt
            );
            //! user no exists
            if (user == null)
                return new UserLoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            //? user exists
            var manageToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_SECRET_KEY);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity( //Claims[]
                    [ //conjunto de afirmaciones estructuradas (claims), describen el contexto de identidad y autorización asociado al token.
                        new(ClaimTypes.Name, user.Username.ToString()),
                        new(ClaimTypes.Role, user.Role)
                    ]
                ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manageToken.CreateToken(tokenDescriptor);
            UserLoginResponseDTO userLoginResponseDTO = new()
            {
                Token = manageToken.WriteToken(token),
                User = user
            };

            return userLoginResponseDTO;
        }

        public async Task<User> Register(UserRegisterDTO dto)
        {
            var passwordEncrypt = GetMD5(dto.Password);
            User user = new()
            {
                Username = dto.Username,
                Name = dto.Name,
                Role = dto.Role
            };

            _db.User.Add(user);
            await _db.SaveChangesAsync();
            user.Password = passwordEncrypt;
            return user;
        }

        //? Encriptar password con MD5
        public static string GetMD5(string value)
        {
            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(value);
            var hash = md5.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}