using Microsoft.EntityFrameworkCore;
using MovieManiaApi.Models;

namespace MovieManiaApi.Models
{
    public class AppDbcontext : DbContext
    {
        public DbSet<Film> Films { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<FilmGenre> FilmGenres { get; set; }
        public DbSet<SerieGenre> SerieGenres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +
                "port=3306;" +
                "user=root;" +
                "password=admin123;" +
                "database=MovieMania;",
            ServerVersion.Parse("5.7.33-winx64")
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Film>().HasData(
                new Film { Id = 1, Title = "The Lord of the Rings: The Fellowship of the Ring", Platform = "Netflix", ReleaseYear = 2001, Rating = 5 },
                new Film { Id = 2, Title = "Inception", Platform = "Amazon Prime", ReleaseYear = 2010, Rating = 4 },
                new Film { Id = 3, Title = "The Dark Knight", Platform = "HBO", ReleaseYear = 2008, Rating = 5 },
                new Film { Id = 4, Title = "Interstellar", Platform = "Netflix", ReleaseYear = 2014, Rating = 4 },
                new Film { Id = 5, Title = "The Avengers", Platform = "Disney+", ReleaseYear = 2012, Rating = 4 },
                new Film { Id = 6, Title = "Toy Story 3", Platform = "Disney+", ReleaseYear = 2010, Rating = 4 },
                new Film { Id = 7, Title = "The Social Network", Platform = "Netflix", ReleaseYear = 2010, Rating = 4 }
            );

            modelBuilder.Entity<Serie>().HasData(
                new Serie { Id = 1, Title = "Game of Thrones", Platform = "HBO", ReleaseYear = 2011, Rating = 4 },
                new Serie { Id = 2, Title = "Breaking Bad", Platform = "Netflix", ReleaseYear = 2008, Rating = 5 }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Fantasy" },
                new Genre { Id = 2, Name = "Action" },
                new Genre { Id = 3, Name = "Science Fiction" },
                new Genre { Id = 4, Name = "Drama" },
                new Genre { Id = 5, Name = "Comedy" },
                new Genre { Id = 6, Name = "Horror" },
                new Genre { Id = 7, Name = "Thriller" },
                new Genre { Id = 8, Name = "Romance" },
                new Genre { Id = 9, Name = "Adventure" }
            );

            modelBuilder.Entity<FilmGenre>().HasData(
                new FilmGenre { Id = 1, FilmId = 1, GenreId = 1 },
                new FilmGenre { Id = 2, FilmId = 1, GenreId = 9 },
                new FilmGenre { Id = 3, FilmId = 2, GenreId = 2 },
                new FilmGenre { Id = 4, FilmId = 2, GenreId = 3 },
                new FilmGenre { Id = 5, FilmId = 3, GenreId = 2 },
                new FilmGenre { Id = 6, FilmId = 3, GenreId = 3 },
                new FilmGenre { Id = 7, FilmId = 4, GenreId = 3 },
                new FilmGenre { Id = 8, FilmId = 4, GenreId = 9 },
                new FilmGenre { Id = 9, FilmId = 5, GenreId = 2 },
                new FilmGenre { Id = 10, FilmId = 5, GenreId = 3 },
                new FilmGenre { Id = 11, FilmId = 6, GenreId = 1 },
                new FilmGenre { Id = 12, FilmId = 6, GenreId = 9 },
                new FilmGenre { Id = 13, FilmId = 7, GenreId = 4 },
                new FilmGenre { Id = 14, FilmId = 7, GenreId = 7 }
            );

            modelBuilder.Entity<SerieGenre>().HasData(
                new SerieGenre { Id = 1, SerieId = 1, GenreId = 1 },
                new SerieGenre { Id = 2, SerieId = 1, GenreId = 4 },
                new SerieGenre { Id = 3, SerieId = 2, GenreId = 2 },
                new SerieGenre { Id = 4, SerieId = 2, GenreId = 4 }
            );
        }

        public DbSet<MovieManiaApi.Models.Serie> Serie { get; set; } = default!;
    }
}
