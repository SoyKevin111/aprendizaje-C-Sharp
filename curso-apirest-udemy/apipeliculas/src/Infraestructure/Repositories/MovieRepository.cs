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

        public bool CreateMovie(Movie movie)
        {
            movie.CreatedAt = DateTime.UtcNow;
            _db.Movie.Add(movie);
            return Save();
        }

        public bool DeleteMovie(Movie movie)
        {
            _db.Movie.Remove(movie);
            return Save();
        }

        public ICollection<Movie> FindAll()
        {
            return _db.Movie.OrderBy(m => m.Name).ToList();
        }

        public Movie FindById(int id)
        {
            return _db.Movie.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Movie> FindMovieByName(string name)
        {
            IQueryable<Movie> query = _db.Movie;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name) || e.Description.Contains(name));
            }
            return query.ToList();
        }

        public ICollection<Movie> FindMoviesByCategoryId(int id)
        {
            return _db.Movie.Include(c => c.Category).Where(m => m.CategoryId == id).ToList();
        }

        public bool IfExistMovieById(int id)
        {
            return _db.Movie.Any(c => c.Id == id);
        }

        public bool IfExistMovieByName(string name)
        {
            return _db.Movie.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        public bool UpdateMovie(Movie movie)
        {
            movie.CreatedAt = DateTime.UtcNow;
            var MovieLoad = _db.Movie.Find(movie.Id);
            if (MovieLoad == null) { return false; }
            _db.Entry(MovieLoad).CurrentValues.SetValues(movie); //!PREFERIBLE MAPEAR o FACTORY
            return Save();
        }
    }
}