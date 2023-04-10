using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class MovieTheather
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public Point Location { get; set; }

    }
}
