using apipeliculas.src.Data;
using apipeliculas.src.Models;
using Microsoft.EntityFrameworkCore;

namespace apipeliculas.src.Repositories.Impl
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AplicationDbContext _db;

        public CategoryRepository(AplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<ICollection<Category>> FindAll()
        {
            return await _db.Category
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category> FindById(int id)
        {
            return await _db.Category
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IfExistCategoryById(int id)
        {
            return await _db.Category.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> IfExistCategoryByName(string name)
        {
            return await _db.Category
                .AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<bool> CreateCategory(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            await _db.Category.AddAsync(category);
            return await Save();
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;

            var categoryLoad = await _db.Category.FindAsync(category.Id);
            if (categoryLoad == null) { return false; }

            _db.Entry(categoryLoad).CurrentValues.SetValues(category); //!PREFERIBLE MAPEAR o FACTORY
            return await Save();
        }

        public async Task<bool> DeleteCategory(Category category)
        {
            _db.Category.Remove(category);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0;
        }
    }
}
