using System;
using System.Linq;
using NzbStation.Data.Entities;
using NzbStation.Tmdb;

namespace NzbStation.Models
{
    public static class MappingExtensions
    {
        public static PagedResultModel<MovieSearchResultModel> MapToModel(this TmdbPagedResponse<TmdbMovieSearch> response, int? size)
        {
            var pageSize = Math.Clamp(size ?? 30, min: 1, max: 200);

            var items = response.Results.Take(pageSize).Select(MapToModel).ToList();

            return new PagedResultModel<MovieSearchResultModel>(items, response.Page, response.TotalPages, response.TotalResults);
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
