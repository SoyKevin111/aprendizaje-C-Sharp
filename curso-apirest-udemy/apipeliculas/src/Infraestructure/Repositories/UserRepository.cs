using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using apipeliculas.src.auth;
using apipeliculas.src.auth.Dtos;
using apipeliculas.src.Data;
using apipeliculas.src.Domain.interfaces;
using apipeliculas.src.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace apipeliculas.src.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AplicationDbContext _db;
        private readonly string _SECRET_KEY;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserRepository(AplicationDbContext db, IConfiguration cfg, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this._db = db;
            this._SECRET_KEY = cfg.GetValue<string>("ApiSettings:Secret_Key");
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._mapper = mapper;
        }

        public async Task<ICollection<AppUser>> FindAll()
        {
            return await _db.AppUser
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }

        public async Task<AppUser> FindById(string id)
        {
            return await _db.AppUser
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> IsUniqueUser(string username)
        {
            var userDB = await _db.AppUser
                .FirstOrDefaultAsync(u => u.UserName == username);

            return userDB == null;
        }

        public async Task<UserLoginResponseDTO> Login(UserLoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null) return new UserLoginResponseDTO() { Token = "", User = null };

            bool isValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isValid) return new UserLoginResponseDTO() { Token = "", User = null };

            //! user no exists
            if (user == null || isValid == false)
                return new UserLoginResponseDTO()
                {
                    Token = "",
                    User = null
                };

            //? user exists
            var roles = await _userManager.GetRolesAsync(user);
            var manageToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_SECRET_KEY);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity( //Claims[]
                    [ //conjunto de afirmaciones estructuradas (claims), describen el contexto de identidad y autorización asociado al token.
                        new(ClaimTypes.Name, user.UserName.ToString()),
                        new(ClaimTypes.Role, roles.FirstOrDefault())
                    ]
                ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manageToken.CreateToken(tokenDescriptor);
            UserLoginResponseDTO userLoginResponseDTO = new()
            {
                Token = manageToken.WriteToken(token),
                User = _mapper.Map<UserDataDTO>(user)
            };

            return userLoginResponseDTO;
        }

        public async Task<UserDataDTO> Register(UserRegisterDTO dto)
        {
            AppUser user = new()
            {
                UserName = dto.Username,
                Email = dto.Username,
                NormalizedEmail = dto.Username.ToUpper(),
                Name = dto.Name, //me olvide agregar el campo XD
                CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd")
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Register"));
                }

                await _userManager.AddToRoleAsync(user, "Admin");

                var userResult = await _db.AppUser
                    .FirstOrDefaultAsync(u => u.UserName == dto.Username);

                return _mapper.Map<UserDataDTO>(userResult);
            }

            return new UserDataDTO();
        }
    }
}
