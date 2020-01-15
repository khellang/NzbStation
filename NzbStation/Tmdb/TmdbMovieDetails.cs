using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NzbStation.Tmdb
{
    public class TmdbMovieDetails : TmdbMovie
    {
        [JsonPropertyName("tagline")]
        public string Tagline { get; set; }

        [JsonPropertyName("homepage")]
        public string Homepage { get; set; }

        [JsonPropertyName("imdb_id")]
        public string ImdbId { get; set; }

        [JsonPropertyName("genres")]
        public IReadOnlyCollection<TmdbGenre> Genres { get; set; }
    }
}