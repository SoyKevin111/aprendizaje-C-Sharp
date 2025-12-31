using apipeliculas.src.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace apipeliculas.src.Persistence.Configuration
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movie");
            builder.HasKey(m => m.Id);
            builder.Property(c => c.Name);
            builder.Property(c => c.Description);
            builder.Property(c => c.Duration);
            builder.Property(c => c.ImageUrl);
            builder.Property(c => c.Clasification).HasConversion<string>();
            builder.Property(c => c.CreatedAt);
            //one to one
            builder
                .HasOne(m => m.Category)
                .WithOne(c => c.MovieData)
                .HasForeignKey<Movie>(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}