using System.Collections.Generic;
using NodaTime;

namespace NzbStation.Models
{
    public class MovieSearchResultModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string OriginalTitle { get; set; }

        public string OriginalLanguage { get; set; }

        public string Overview { get; set; }

        public LocalDate? ReleaseDate { get; set; }

        public double Popularity { get; set; }

        public double VoteAverage { get; set; }

        public int VoteCount { get; set; }
    }
}
