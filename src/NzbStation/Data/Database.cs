using Microsoft.EntityFrameworkCore;
using NzbStation.Data.Entities;

namespace NzbStation.Data
{
    public class Database : DbContext
    {
        public Database(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=NzbStation.db");
            options.UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Movie>().Property(x => x.Id).ValueGeneratedNever();
            model.Entity<Movie>().HasIndex(x => x.Slug);
            model.Entity<Movie>().Property(x => x.ReleaseDate).HasConversion(LocalDateValueConverter.Instance);

            model.Entity<Genre>().Property(x => x.Id).ValueGeneratedNever();

            model.Entity<MovieGenre>().HasKey(x => new { x.MovieId, x.GenreId });
        }
    }
}
