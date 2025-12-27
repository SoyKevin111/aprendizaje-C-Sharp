using apipeliculas.src.Models;
using Microsoft.EntityFrameworkCore; //entity framework core

namespace apipeliculas.src.Data
{
    public class AplicationDbContext : DbContext
    {
        //atajo ctor
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }
        //pasar todas las entidades
        public DbSet<Category> Category { get; set; }
    }
}