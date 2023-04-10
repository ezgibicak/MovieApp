using MoviesAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dto
{
    public class GenreCreationDTO
    {
        [Required(ErrorMessage = "This is required")]
        [StringLength(50)]
        [FirstUpperCaseValidation]
        public string Name { get; set; }
    }
}
