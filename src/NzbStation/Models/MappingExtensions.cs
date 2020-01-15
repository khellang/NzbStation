using System.Linq;
using NzbStation.Data.Entities;
using NzbStation.Tmdb;

namespace NzbStation.Models
{
    public static class MappingExtensions
    {
        public static PagedResultModel<MovieSearchResultModel> MapToModel(this TmdbPagedResponse<TmdbMovieSearch> response)
        {
            return new PagedResultModel<MovieSearchResultModel>
            {
                Page = response.Page,
                TotalPages = response.TotalPages,
                TotalResults = response.TotalResults,
                Results = response.Results.Select(MapToModel).ToList(),
            };
        }

        public static MovieSearchResultModel MapToModel(this TmdbMovieSearch movie)
        {
            return new MovieSearchResultModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Overview = movie.Overview,
                Popularity = movie.Popularity,
                OriginalTitle = movie.OriginalTitle,
                OriginalLanguage = movie.OriginalLanguage,
                ReleaseDate = movie.ReleaseDate,
                VoteAverage = movie.VoteAverage,
                VoteCount = movie.VoteCount
            };
        }

        public static MovieDetailsModel MapToModel(this Movie movie)
        {
            return new MovieDetailsModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Overview = movie.Overview,
                Slug = movie.Slug,
                OriginalTitle = movie.OriginalTitle,
                SortTitle = movie.SortTitle,
                ReleaseDate = movie.ReleaseDate,
                Tagline = movie.Tagline,
                Homepage = movie.Homepage,
                ImdbId = movie.ImdbId,
            };
        }
    }
}
