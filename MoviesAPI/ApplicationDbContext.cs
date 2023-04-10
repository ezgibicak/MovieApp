using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Entities;

namespace MoviesAPI
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesActors>().HasKey(x => new { x.ActorId, x.MovieId });
            modelBuilder.Entity<MoviesGenres>().HasKey(x => new { x.MovieId, x.GenreId });
            modelBuilder.Entity<MovieAndMovieTheater>().HasKey(x => new { x.MovieTheatherId, x.MovieId });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieTheather> MovieTheathers { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MovieAndMovieTheater> MovieAndMovieTheater { get; set; }
        public DbSet<Rating> Rating { get; set; }

    }
}
