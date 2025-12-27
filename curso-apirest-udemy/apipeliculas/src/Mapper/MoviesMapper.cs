using apipeliculas.src.Dtos;
using apipeliculas.src.Models;
using AutoMapper;

namespace apipeliculas.src.Mapper
{
    public class MoviesMapper : Profile
    {
        public MoviesMapper()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
        }
    }
}