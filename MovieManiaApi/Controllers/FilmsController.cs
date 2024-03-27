using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieManiaApi.Models;

namespace MovieManiaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public FilmsController(AppDbcontext context)
        {
            _context = context;
        }

        // GET: api/Films
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDto>>> GetFilms()
        {
            var films = await _context.Films
                .Include(f => f.FilmGenres)
                    .ThenInclude(fg => fg.Genre)
                .ToListAsync();

            var filmDtos = new List<FilmDto>();

            foreach (var film in films)
            {
                var genreNames = film.FilmGenres.Select(fg => fg.Genre.Name).ToList();

                filmDtos.Add(new FilmDto
                {
                    Id = film.Id,
                    Title = film.Title,
                    Platform = film.Platform,
                    Rating = film.Rating,
                    ReleaseYear = film.ReleaseYear,
                    Genres = genreNames
                });
            }

            return filmDtos;
        }

        // GET: api/Films/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
          if (_context.Films == null)
          {
              return NotFound();
          }
            var film = await _context.Films.FindAsync(id);

            if (film == null)
            {
                return NotFound();
            }

            return film;
        }

        // PUT: api/Films/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(int id, FilmDto filmDto)
        {
            if (id != filmDto.Id)
            {
                return BadRequest();
            }

            var film = await _context.Films
                .Include(f => f.FilmGenres)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (film == null)
            {
                return NotFound();
            }

            film.FilmGenres.Clear();

            film.Title = filmDto.Title;
            film.Platform = filmDto.Platform;
            film.ReleaseYear = filmDto.ReleaseYear;
            film.Rating = filmDto.Rating;

            foreach (var genreName in filmDto.Genres)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (genre != null)
                {
                    var filmGenre = new FilmGenre
                    {
                        FilmId = film.Id,
                        GenreId = genre.Id
                    };
                    film.FilmGenres.Add(filmGenre);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Films
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(FilmDto filmDto)
        {
            if (filmDto == null)
            {
                return BadRequest("Film data is null.");
            }

            var film = new Film
            {
                Title = filmDto.Title,
                Platform = filmDto.Platform,
                ReleaseYear = filmDto.ReleaseYear,
                Rating = filmDto.Rating
            };

            _context.Films.Add(film);
            await _context.SaveChangesAsync();

            foreach (var genreName in filmDto.Genres)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (genre != null)
                {
                    var filmGenre = new FilmGenre
                    {
                        FilmId = film.Id,
                        GenreId = genre.Id
                    };
                    _context.FilmGenres.Add(filmGenre);
                }
            }

            await _context.SaveChangesAsync();

            var filmDtoResponse = new FilmDto
            {
                Id = film.Id,
                Title = film.Title,
                Platform = film.Platform,
                ReleaseYear = film.ReleaseYear,
                Rating = film.Rating,
                Genres = filmDto.Genres
            };

            return CreatedAtAction("GetFilm", new { id = film.Id }, filmDtoResponse);
        }

        // DELETE: api/Films/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            if (_context.Films == null)
            {
                return NotFound();
            }
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FilmExists(int id)
        {
            return (_context.Films?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
