using System.Collections.Generic;
using NodaTime;

namespace NzbStation.Data.Entities
{
    public class Movie
    {
        public Movie()
        {
            Genres = new List<MovieGenre>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string SortTitle { get; set; }

        public string OriginalTitle { get; set; }

        public string Slug { get; set; }

        public string Tagline { get; set; }

        public string Homepage { get; set; }

        public string ImdbId { get; set; }

        public string Overview { get; set; }

        public LocalDate? ReleaseDate { get; set; }

        public ICollection<MovieGenre> Genres { get; }
    }
}
