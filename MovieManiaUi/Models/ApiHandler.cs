using MovieManiaUi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieManiaUi.Models
{
    internal class ApiHandler
    {
        private readonly HttpClient _client;

        public ApiHandler()
        {
            _client = new HttpClient();
        }

        public async Task<List<Film>> GetFilmsAsync()
        {
            string url = "https://localhost:7193/api/Films";

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<Film>>(content, options);
        }

        public async Task<List<Serie>> GetSeriesAsync()
        {
            string url = "https://localhost:7193/api/Series";

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<Serie>>(content, options);
        }

        public async Task<List<Genre>> GetGenresAsync()
        {
            string url = "https://localhost:7193/api/Genres";

            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<Genre>>(content, options);
        }
    }
}
