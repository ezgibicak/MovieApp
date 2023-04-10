using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dto
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public bool Intheaters { get; set; }
        public string Poster { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<MovieTheatherDTO> MovieTheathers { get; set; }
        public List<ActorsMovieDTO> Actors { get; set; }
        public List<GenreDTO> Genres { get; set; }
        public double AvarageVote { get; set; }
        public int UserVote { get; set; }

}
}
