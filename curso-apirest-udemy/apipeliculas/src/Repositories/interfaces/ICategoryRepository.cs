using apipeliculas.src.Models;

namespace apipeliculas.src.Repositories
{
    public interface ICategoryRepository
    {
        ICollection<Category> FindAll();
        Category FindById(int id);
        bool IfExistCategoryById(int id);
        bool IfExistCategoryByName(string name);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}