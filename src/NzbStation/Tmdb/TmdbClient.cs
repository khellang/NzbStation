using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using NodaTime.Text;
using NzbStation.Extensions;

namespace NzbStation.Tmdb
{
    public class TmdbClient
    {
        static TmdbClient()
        {
            Options = new JsonSerializerOptions
            {
                Converters = { new CustomLocalDateConverter() },
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true,
            };
        }

        public TmdbClient(HttpClient client)
        {
            Client = client;
        }

        private static JsonSerializerOptions Options { get; }

        private HttpClient Client { get; }

        public async Task<IReadOnlyCollection<TmdbGenre>> GetMovieGenresAsync(CancellationToken cancellationToken)
        {
            using var response = await Client.GetAsync("/3/genre/movie/list", cancellationToken);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadJsonAsync<TmdbGenreResponse>(Options, cancellationToken);

            return content.Genres;
        }

        public async Task<TmdbMovieDetails> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
        {
            using var response = await Client.GetAsync($"/3/movie/{id}", cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadJsonAsync<TmdbMovieDetails>(Options, cancellationToken);
        }

        public async Task<TmdbPagedResponse<TmdbMovieSearch>> SearchMoviesAsync(string query, int page, CancellationToken cancellationToken)
        {
            var escapedQuery = Uri.EscapeUriString(query);

            var requestUri = $"/3/search/movie?query={escapedQuery}&page={page}";

            var response = await Client.GetAsync(requestUri, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadJsonAsync<TmdbPagedResponse<TmdbMovieSearch>>(Options, cancellationToken);
        }

        private class TmdbGenreResponse
        {
            [JsonPropertyName("genres")]
            public IReadOnlyCollection<TmdbGenre> Genres { get; set; }
        }

        private sealed class CustomLocalDateConverter : NodaConverterBase<LocalDate?>
        {
            private static readonly IPattern<LocalDate> Pattern = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd");

            protected override LocalDate? ReadJsonImpl(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                var text = reader.GetString();

                if (string.IsNullOrEmpty(text))
                {
                    return default;
                }

                return Pattern.Parse(text).Value;
            }

            protected override void WriteJsonImpl(Utf8JsonWriter writer, LocalDate? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                {
                    writer.WriteStringValue(Pattern.Format(value.Value));
                    return;
                }

                writer.WriteNullValue();
            }
        }
    }
}
