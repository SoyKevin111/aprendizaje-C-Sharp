using apipeliculas.src.Dtos;
using apipeliculas.src.Models;
using apipeliculas.src.Repositories;
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
        [ResponseCache(CacheProfileName = "CachePorDefault30")]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)] //no se guarde cache, ni cliente ni servidor
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories() //XD
        {
            var categories = await _categoryRepository.FindAll();

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
        public async Task<IActionResult> GetCategorieById(int id)
        {
            var categorie = await _categoryRepository.FindById(id);
            if (categorie == null)
            {
                return NotFound();
            }

            var categorieDto = _mapper.Map<CategoryDTO>(categorie);
            return Ok(categorieDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (dto == null) { return BadRequest(ModelState); }

            if (await _categoryRepository.IfExistCategoryByName(dto.Name))
            {
                ModelState.AddModelError("[Conflict Error]", "Categoria existente.");
                return StatusCode(409, ModelState); // conflict
            }

            var category = _mapper.Map<Category>(dto);

            if (!await _categoryRepository.CreateCategory(category))
            {
                ModelState.AddModelError("[Create Error]", "Error al crear la categoria.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}", Name = "UpdatePatchCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePatchCategory(int id, [FromBody] CategoryDTO dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (dto == null || id != dto.Id) { return BadRequest(ModelState); }

            var category = _mapper.Map<Category>(dto);

            if (!await _categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("[Update Error]", "Error al actualizar la categoria.");
                return StatusCode(404, ModelState);
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDTO dto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (dto == null || id != dto.Id) { return BadRequest(ModelState); }

            var categoryLoad = await _categoryRepository.FindById(id);
            if (categoryLoad == null)
            {
                return NotFound("No se encontro la categoria.");
            }

            var category = _mapper.Map<Category>(dto);

            if (!await _categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("[Update Error]", "Error al actualizar la categoria.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!await _categoryRepository.IfExistCategoryById(id))
            {
                return NotFound();
            }

            var category = await _categoryRepository.FindById(id);

            if (!await _categoryRepository.DeleteCategory(category!))
            {
                ModelState.AddModelError("[Delete Error]", "Error al borrar la categoria.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
