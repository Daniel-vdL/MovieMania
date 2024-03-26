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
    public class FilmGenresController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public FilmGenresController(AppDbcontext context)
        {
            _context = context;
        }

        // POST: api/FilmGenres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FilmGenre>> PostFilmGenre(FilmGenre filmGenre)
        {
          if (_context.FilmGenres == null)
          {
              return Problem("Entity set 'AppDbcontext.FilmGenres'  is null.");
          }
            _context.FilmGenres.Add(filmGenre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFilmGenre", new { id = filmGenre.Id }, filmGenre);
        }

        private bool FilmGenreExists(int id)
        {
            return (_context.FilmGenres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
