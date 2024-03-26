namespace MovieManiaApi.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Film>? Films { get; set; }
        public ICollection<Serie>? Series { get; set; }
    }

    public class GenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
