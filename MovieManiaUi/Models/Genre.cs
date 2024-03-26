using MovieManiaUi.Models;
using System.Collections.Generic;

namespace MovieManiaUi.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Film> Films { get; set; }
    }
}
