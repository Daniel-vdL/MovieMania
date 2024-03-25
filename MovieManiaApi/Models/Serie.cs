namespace MovieManiaApi.Models
{
    public class Serie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public int ReleaseYear { get; set; }
        public int Rating { get; set; }

        public ICollection<Genre> Genres { get; set; }

    }
}
