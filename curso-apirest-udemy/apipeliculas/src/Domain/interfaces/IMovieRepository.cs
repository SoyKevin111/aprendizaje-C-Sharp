using apipeliculas.src.Models;

namespace apipeliculas.src.Repositories
{
    public interface IMovieRepository
    {
        Task<ICollection<Movie>> FindAll();
        Task<ICollection<Movie>> FindMoviesByCategoryId(int id);
        Task<ICollection<Movie>> FindMovieByName(string name);
        Task<Movie> FindById(int id);
        Task<bool> IfExistMovieById(int id);
        Task<bool> IfExistMovieByName(string name);
        Task<bool> CreateMovie(Movie movie);
        Task<bool> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(Movie movie);
    }
}
