using System.ComponentModel.DataAnnotations;

namespace NzbStation.Models
{
    public class AddMovieModel
    {
        [Required]
        public int? Id { get; set; }
    }
}
