using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NzbStation.Tmdb
{
    public class TmdbPagedResponse<T>
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("results")]
        public IReadOnlyCollection<T> Results { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }
}
