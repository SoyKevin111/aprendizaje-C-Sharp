using System.Net;
using apipeliculas.src.auth;
using apipeliculas.src.Domain.Dtos;
using apipeliculas.src.Domain.interfaces;
using apipeliculas.src.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apipeliculas.src.Infraestructure.Controllers
{
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _uRepo;
        private readonly IMapper _mapper;
        private readonly ResponseAPI _resAPI;

        public UserController(IUserRepository uRepo, IMapper mapper)
        {
            this._uRepo = uRepo;
            this._mapper = mapper;
            this._resAPI = new ResponseAPI();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _uRepo.FindAll();

            var userDto = new List<UserDTO>();
            foreach (var u in users)
            {
                userDto.Add(_mapper.Map<UserDTO>(u));
            }

            return Ok(userDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _uRepo.FindById(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO registerDTO)
        {
            bool isUniqueUser = await _uRepo.IsUniqueUser(registerDTO.Username);
            if (!isUniqueUser)
            {
                _resAPI.StatusCode = HttpStatusCode.BadRequest;
                _resAPI.IsSucess = false;
                _resAPI.ErrorMessages.Add("Usuario ya existente.");
                return BadRequest(_resAPI);
            }

            var user = await _uRepo.Register(registerDTO);
            if (user == null)
            {
                _resAPI.StatusCode = HttpStatusCode.BadRequest;
                _resAPI.IsSucess = false;
                _resAPI.ErrorMessages.Add("Error al registrar el usuario.");
                return BadRequest(_resAPI);
            }

            _resAPI.StatusCode = HttpStatusCode.OK;
            _resAPI.IsSucess = true;
            return Ok(_resAPI);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            var loginResponse = await _uRepo.Login(loginDTO);

            if (loginResponse.User == null && string.IsNullOrEmpty(loginResponse.Token))
            {
                _resAPI.StatusCode = HttpStatusCode.BadRequest;
                _resAPI.IsSucess = false;
                _resAPI.ErrorMessages.Add("Credenciales Incorrectas.");
                return BadRequest(_resAPI);
            }

            _resAPI.StatusCode = HttpStatusCode.OK;
            _resAPI.IsSucess = true;
            _resAPI.Result = loginResponse;
            return Ok(_resAPI);
        }
    }
}
