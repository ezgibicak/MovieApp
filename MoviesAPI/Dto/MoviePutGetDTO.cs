namespace MoviesAPI.Dto
{
    public class MoviePutGetDTO
    {
        public MovieDTO Movie { get; set; }
        public List<GenreDTO> SelectedGenres { get; set; }
        public List<GenreDTO> NonSelectedGenres { get; set; }
        public List<MovieTheatherDTO> SelectedMovieTheathers { get; set; }
        public List<MovieTheatherDTO> NonSelectedMovieTheathers { get; set; }
        public List<ActorsMovieDTO> Actors { get; set; }
    }
}
