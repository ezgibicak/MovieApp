using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dto
{
    public class ActorCreationDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
