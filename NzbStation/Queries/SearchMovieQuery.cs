using System.Threading;
using System.Threading.Tasks;
using NzbStation.Models;
using NzbStation.Tmdb;

namespace NzbStation.Queries
{
    public class SearchMovieQuery : IQuery<PagedResultModel<MovieSearchResultModel>>
    {
        public SearchMovieQuery(string query, int page)
        {
            Query = query;
            Page = page;
        }

        public string Query { get; }

        public int Page { get; }

        public class Handler : IQueryHandler<SearchMovieQuery, PagedResultModel<MovieSearchResultModel>>
        {
            public Handler(TmdbClient client)
            {
                Client = client;
            }

            private TmdbClient Client { get; }

            public async Task<PagedResultModel<MovieSearchResultModel>> ExecuteAsync(SearchMovieQuery query, CancellationToken cancellationToken)
            {
                var result = await Client.SearchMoviesAsync(query.Query, query.Page, cancellationToken);

                return result.MapToModel();
            }
        }
    }
}