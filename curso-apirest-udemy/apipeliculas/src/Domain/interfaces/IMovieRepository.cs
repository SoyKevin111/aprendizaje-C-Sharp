using apipeliculas.src.Models;

namespace apipeliculas.src.Repositories
{
    public interface IMovieRepository
    {
        ICollection<Movie> FindAll();
        ICollection<Movie> FindMoviesByCategoryId(int id);
        IEnumerable<Movie> FindMovieByName(string name);
        Movie FindById(int id);
        bool IfExistMovieById(int id);
        bool IfExistMovieByName(string name);
        bool CreateMovie(Movie movie);
        bool UpdateMovie(Movie movie);
        bool DeleteMovie(Movie movie);
        bool Save();
    }
}