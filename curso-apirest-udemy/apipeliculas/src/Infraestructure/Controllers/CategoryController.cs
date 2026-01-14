using apipeliculas.src.Dtos;
using apipeliculas.src.Models;
using apipeliculas.src.Repositories;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace apipeliculas.src.Controllers
{
    //[Authorize(Roles = "Admin")]

    [Route("api/categories")]
    [ApiController]
    [EnableCors("PoliticaCors")]
    public class CategoryController : ControllerBase //rest
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        [ResponseCache(CacheProfileName = "CachePorDefault30")]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] //no se guarde cache, ni cliente ni servidor
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[EnableCors("PoliticaCors")]
        public IActionResult GetCategories() //XD
        {
            var categories = _categoryRepository.FindAll();
            var categoriesDto = new List<CategoryDTO>();
            foreach (var c in categories)
            {
                categoriesDto.Add(_mapper.Map<CategoryDTO>(c));
            }
            return Ok(categoriesDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorieById(int id)
        {
            var categorie = _categoryRepository.FindById(id);
            if (categorie == null)
            {
                return NotFound();
            }
            var categorieDto = _mapper.Map<CategoryDTO>(categorie);
            return Ok(categorieDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (dto == null) { return BadRequest(ModelState); }
            if (_categoryRepository.IfExistCategoryByName(dto.Name))
            {
                ModelState.AddModelError("[Conflict Error]", "Categoria existente.");
                return StatusCode(404, ModelState);
            }
            var category = _mapper.Map<Category>(dto);
            if (!_categoryRepository.CreateCategory(category))
            {
                ModelState.AddModelError("[Create Error]", "Error al crear la categoria.");
                return StatusCode(404, ModelState);
            }
            return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}", Name = "UpdatePatchCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePatchCategory(int id, [FromBody] CategoryDTO dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (dto == null || id != dto.Id) { return BadRequest(ModelState); }

            var category = _mapper.Map<Category>(dto);
            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("[Update Error]", "Error al actualizar la categoria.");
                return StatusCode(404, ModelState);
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDTO dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (dto == null || id != dto.Id) { return BadRequest(ModelState); }

            var categoryLoad = _categoryRepository.FindById(id);
            if (categoryLoad == null) { return NotFound("No se encontro la categoria."); }

            var category = _mapper.Map<Category>(dto);
            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("[Update Error]", "Error al actualizar la categoria.");
                return StatusCode(404, ModelState);
            }
            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        public IActionResult DeleteCategory(int id)
        {
            if (_categoryRepository.IfExistCategoryById(id)) { return NotFound(); }
            var category = _categoryRepository.FindById(id);

            if (_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("[Delete Error]", "Error al borrar la categoria.");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

    }
}