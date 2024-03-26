using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManiaUi.Models
{
    internal class Serie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public int ReleaseYear { get; set; }
        public int Rating { get; set; }
        public List<string> Genres { get; set; }

        public string GenresAsString => string.Join(", ", Genres);
    }
}
