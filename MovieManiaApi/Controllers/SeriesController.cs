﻿using System;
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
    public class SeriesController : ControllerBase
    {
        private readonly AppDbcontext _context;

        public SeriesController(AppDbcontext context)
        {
            _context = context;
        }

        // GET: api/Series
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SerieDto>>> GetSeries()
        {
            var series = await _context.Series
                .Include(sg => sg.SerieGenres)
                    .ThenInclude(sg => sg.Genre)
                .ToListAsync();

            var serieDtos = new List<SerieDto>();

            foreach (var serie in series) 
            { 
                var genreNames = serie.SerieGenres.Select(sg => sg.Genre.Name).ToList();

                serieDtos.Add(new SerieDto 
                {
                    Id = serie.Id,
                    Title = serie.Title,
                    Platform = serie.Platform,
                    ReleaseYear = serie.ReleaseYear,
                    Rating = serie.Rating,
                    Genres = genreNames
                });
            }

            return serieDtos;
        }

        // GET: api/Series/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Serie>> GetSerie(int id)
        {
          if (_context.Series == null)
          {
              return NotFound();
          }
            var serie = await _context.Series.FindAsync(id);

            if (serie == null)
            {
                return NotFound();
            }

            return serie;
        }

        // PUT: api/Series/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSerie(int id, SerieDto serieDto)
        {
            if (id != serieDto.Id)
            {
                return BadRequest();
            }

            var serie = await _context.Series
                .Include(s => s.SerieGenres)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (serie == null)
            {
                return NotFound();
            }

            serie.SerieGenres.Clear();

            serie.Title = serieDto.Title;
            serie.Platform = serieDto.Platform;
            serie.ReleaseYear = serieDto.ReleaseYear;
            serie.Rating = serieDto.Rating;

            foreach (var genreName in serieDto.Genres)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (genre != null)
                {
                    var serieGenre = new SerieGenre
                    {
                        SerieId = serie.Id,
                        GenreId = genre.Id
                    };
                    serie.SerieGenres.Add(serieGenre);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SerieExists(id))
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

        // POST: api/Series
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Serie>> PostSerie(SerieDto serieDto)
        {
            if (serieDto == null)
            {
                return BadRequest("Serie data is null.");
            }

            var serie = new Serie
            {
                Title = serieDto.Title,
                Platform = serieDto.Platform,
                ReleaseYear = serieDto.ReleaseYear,
                Rating = serieDto.Rating
            };

            _context.Series.Add(serie);
            await _context.SaveChangesAsync();

            if (serieDto.Genres != null && serieDto.Genres.Any())
            {
                foreach (var genreName in serieDto.Genres)
                {
                    var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                    if (genre != null)
                    {
                        var serieGenre = new SerieGenre
                        {
                            SerieId = serie.Id,
                            GenreId = genre.Id
                        };
                        _context.SerieGenres.Add(serieGenre);
                    }
                }
                await _context.SaveChangesAsync();
            }

            var serieDtoResponse = new SerieDto
            {
                Id = serie.Id,
                Title = serie.Title,
                Platform = serie.Platform,
                ReleaseYear = serie.ReleaseYear,
                Rating = serie.Rating,
                Genres = serieDto.Genres
            };

            return CreatedAtAction("GetSerie", new { id = serie.Id }, serieDtoResponse);
        }

        // DELETE: api/Series/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSerie(int id)
        {
            if (_context.Series == null)
            {
                return NotFound();
            }
            var serie = await _context.Series.FindAsync(id);
            if (serie == null)
            {
                return NotFound();
            }

            _context.Series.Remove(serie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SerieExists(int id)
        {
            return (_context.Series?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
