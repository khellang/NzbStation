using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NzbStation.Tmdb
{
    public class TmdbMovieSearch : TmdbMovie
    {
        [JsonPropertyName("genre_ids")]
        public IReadOnlyCollection<int> GenreIds { get; set; }
    }
}
