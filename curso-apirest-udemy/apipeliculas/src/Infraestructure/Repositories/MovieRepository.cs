using apipeliculas.src.Data;
using apipeliculas.src.Models;
using Microsoft.EntityFrameworkCore;

namespace apipeliculas.src.Repositories.Impl
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AplicationDbContext _db;

        public MovieRepository(AplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<bool> CreateMovie(Movie movie)
        {
            movie.CreatedAt = DateTime.UtcNow;
            await _db.Movie.AddAsync(movie);
            return await Save();
        }

        public async Task<bool> DeleteMovie(Movie movie)
        {
            _db.Movie.Remove(movie);
            return await Save();
        }

        public async Task<ICollection<Movie>> FindAll()
        {
            return await _db.Movie
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Movie> FindById(int id)
        {
            return await _db.Movie
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ICollection<Movie>> FindMovieByName(string name)
        {
            IQueryable<Movie> query = _db.Movie;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e =>
                    e.Name.Contains(name) ||
                    e.Description.Contains(name));
            }

            return await query.ToListAsync();
        }

        public async Task<ICollection<Movie>> FindMoviesByCategoryId(int id)
        {
            return await _db.Movie
                .Include(c => c.Category)
                .Where(m => m.CategoryId == id)
                .ToListAsync();
        }

        public async Task<bool> IfExistMovieById(int id)
        {
            return await _db.Movie.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> IfExistMovieByName(string name)
        {
            return await _db.Movie
                .AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<bool> UpdateMovie(Movie movie)
        {
            movie.CreatedAt = DateTime.UtcNow;

            var MovieLoad = await _db.Movie.FindAsync(movie.Id);
            if (MovieLoad == null) { return false; }

            _db.Entry(MovieLoad).CurrentValues.SetValues(movie); //!PREFERIBLE MAPEAR o FACTORY
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() >= 0;
        }
    }
}
