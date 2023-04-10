namespace MoviesAPI.Dto
{
    public class MoviePostGetDTO
    {
        public List<GenreDTO> Genres { get; set; }
        public List<MovieTheatherDTO> MovieTheathers { get; set; }
    }
}
