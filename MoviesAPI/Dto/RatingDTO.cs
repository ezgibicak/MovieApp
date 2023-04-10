using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dto
{
    public class RatingDTO
    {
        [Range(1,5)]
        public int Rate { get; set; }
        public int MovieId { get; set; }

    }
}
