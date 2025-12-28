using apipeliculas.src.Dtos;
using apipeliculas.src.Models;
using apipeliculas.src.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace apipeliculas.src.Controllers
{
    [Route("api/categories")]
    public class CategoryController : ControllerBase //rest
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.FindAll();
            var categoriesDto = new List<CategoryDTO>();
            foreach (var c in categories)
            {
                categoriesDto.Add(_mapper.Map<CategoryDTO>(c));
            }
            return Ok(categoriesDto);
        }

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