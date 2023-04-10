namespace MoviesAPI.Entities
{
    public class MovieAndMovieTheater
    {
        public int MovieId { get; set; }
        public int MovieTheatherId { get; set; }
        public Movie Movie { get; set; }
        public MovieTheather MovieTheather { get; set; }
    }
}
