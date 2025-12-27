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
        bool UpdateCateogry(Category category);
        bool Save();
    }
}