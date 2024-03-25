using MovieManiaApi.Models;
using System.Collections.Generic;

namespace MovieManiaUi.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public int ReleaseYear { get; set; }
        public int Rating { get; set; }
        public List<string> Genres { get; set; }
    }
}
