using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dto
{
    public class UserCredentials
    {
        [Required]
        public string Password { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
