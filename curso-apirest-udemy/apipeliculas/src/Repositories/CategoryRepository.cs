using apipeliculas.src.Data;
using apipeliculas.src.Models;

namespace apipeliculas.src.Repositories.Impl
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AplicationDbContext _db;

        public CategoryRepository(AplicationDbContext db)
        {
            this._db = db;
        }

        public bool CreateCategory(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            _db.Category.Add(category);
            return Save();
        }

        public ICollection<Category> FindAll()
        {
            return _db.Category.OrderBy(c => c.Name).ToList();
        }

        public Category FindById(int id)
        {
            return _db.Category.FirstOrDefault(c => c.Id == id);
        }

        public bool IfExistCategoryById(int id)
        {
            return _db.Category.Any(c => c.Id == id);
        }

        public bool IfExistCategoryByName(string name)
        {
            return _db.Category.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        public bool UpdateCateogry(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            _db.Category.Update(category);
            return Save();
        }
    }
}