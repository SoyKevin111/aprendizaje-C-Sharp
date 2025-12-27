using apipeliculas.src.Dtos;
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
    }
}