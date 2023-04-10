using MoviesAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Genre /*: IValidatableObject*/
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This is required")]
        [StringLength(50)]
        [FirstUpperCaseValidation]
        public string Name { get; set; }
        //[Range(18, 120)]
        //public int Age { get; set; }

        //[CreditCard]
        //public int CreditCard { get; set; }

        //[Url]
        //public int Url { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (!string.IsNullOrEmpty(Name))
        //    {
        //        var firstLetter = Name[0].ToString();
        //        if (firstLetter != firstLetter.ToUpper())
        //        {
        //            yield return new ValidationResult("First letter must be upper", new string[] { nameof(Name) });

        //        }
        //    }
        //}
    }
}
