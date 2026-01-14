using apipeliculas.src.Domain.interfaces;
using apipeliculas.src.Dtos;
using apipeliculas.src.Models;
using apipeliculas.src.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apipeliculas.src.Infraestructure.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _mvRepo;
        private readonly IMapper _mapper;
        private readonly IImageStoreService _imgStoreService;
        private readonly ILogger<MovieController> _logger;

        public MovieController(IMovieRepository movieRepository, IMapper mapper, ILogger<MovieController> logger, IImageStoreService imageStoreService)
        {
            this._mvRepo = movieRepository;
            this._mapper = mapper;
            this._imgStoreService = imageStoreService;
            this._logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _mvRepo.FindAll();

            var moviesDTO = new List<MovieDTO>();
            foreach (var m in movies)
            {
                moviesDTO.Add(_mapper.Map<MovieDTO>(m));
            }

            return Ok(moviesDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}", Name = "GetMovieById")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _mvRepo.FindById(id);
            if (movie == null)
            {
                return NotFound();
            }

            var movieDTO = _mapper.Map<CategoryDTO>(movie);
            return Ok(movieDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovieDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDTO dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (dto == null) { return BadRequest(ModelState); }

            if (await _mvRepo.IfExistMovieByName(dto.Name))
            {
                ModelState.AddModelError("[Conflict Error]", "Pelicula existente.");
                return StatusCode(404, ModelState);
            }

            var movie = _mapper.Map<Movie>(dto);

            if (!await _mvRepo.CreateMovie(movie))
            {
                ModelState.AddModelError("[Create Error]", "Error al crear la pelicula.");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetMovieById", new { id = movie.Id }, movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}", Name = "UpdatePatchMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePatchMovie(int id, [FromBody] MovieDTO dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (dto == null || id != dto.Id) { return BadRequest(ModelState); }

            var movie = _mapper.Map<Movie>(dto);

            if (!await _mvRepo.UpdateMovie(movie))
            {
                ModelState.AddModelError("[Update Error]", "Error al actualizar la pelicula.");
                return StatusCode(404, ModelState);
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}", Name = "DeleteMovie")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (!await _mvRepo.IfExistMovieById(id))
            {
                return NotFound();
            }

            var movie = await _mvRepo.FindById(id);

            if (!await _mvRepo.DeleteMovie(movie))
            {
                ModelState.AddModelError("[Delete Error]", "Error al borrar la pelicula.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("search/movie/category/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMoviesByCategory(int categoryId)
        {
            var movies = await _mvRepo.FindMoviesByCategoryId(categoryId);
            if (!movies.Any())
            {
                return NotFound();
            }

            var items = new List<MovieDTO>();
            foreach (var m in movies)
            {
                items.Add(_mapper.Map<MovieDTO>(m));
            }

            _logger.LogInformation("Movies: {@Movies}", movies);
            return Ok(items);
        }

        [AllowAnonymous]
        [HttpGet("search/movie")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchMovies(string name)
        {
            try
            {
                var result = await _mvRepo.FindMovieByName(name);
                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost("upload/img")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            var result = await _imgStoreService.UploadImageAsync(file);
            return Ok(result);
        }
    }
}
