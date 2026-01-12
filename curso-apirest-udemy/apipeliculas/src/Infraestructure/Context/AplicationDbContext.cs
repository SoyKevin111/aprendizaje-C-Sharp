using apipeliculas.src.Domain.Models;
using apipeliculas.src.Models;
using apipeliculas.src.Persistence.Configuration;
using Microsoft.EntityFrameworkCore; //entity framework core

namespace apipeliculas.src.Data
{
    public class AplicationDbContext(DbContextOptions<AplicationDbContext> options) : DbContext(options)
    {
        //pasar todas las entidades
        public DbSet<Category> Category { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
        }
    }
}