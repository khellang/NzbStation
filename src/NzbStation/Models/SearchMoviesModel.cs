using System.ComponentModel.DataAnnotations;

namespace NzbStation.Models
{
    public class SearchMoviesModel
    {
        [Required]
        public string Q { get; set; }

        public int? Page { get; set; }
    }
}
