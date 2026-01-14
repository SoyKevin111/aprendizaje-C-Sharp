using apipeliculas.src.Domain.Models;
using apipeliculas.src.Models;
using apipeliculas.src.Persistence.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore; //entity framework core

namespace apipeliculas.src.Data
{
    public class AplicationDbContext : IdentityDbContext<AppUser>
    {

        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) { }



        //pasar todas las entidades
        public DbSet<Category> Category { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<AppUser> AppUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
        }
    }
}