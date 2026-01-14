using apipeliculas.src.Models;

namespace apipeliculas.src.Repositories
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> FindAll();
        Task<Category> FindById(int id);
        Task<bool> IfExistCategoryById(int id);
        Task<bool> IfExistCategoryByName(string name);
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(Category category);
    }
}
