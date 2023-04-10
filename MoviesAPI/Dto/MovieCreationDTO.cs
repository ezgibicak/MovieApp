using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dto
{
    public class MovieCreationDTO
    {
        [StringLength(100)]
        [Required]
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public bool Intheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenresIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> MovieTheaterIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<MoviesActorsCreationDTO>>))]
        public List<MoviesActorsCreationDTO> Actors { get; set; }

        

    }
}
