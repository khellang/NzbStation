using System.Threading;
using System.Threading.Tasks;
using NzbStation.Models;
using NzbStation.Tmdb;
using Zynapse;

namespace NzbStation.Queries
{
    public class SearchMovieQuery : PagedQuery<MovieSearchResultModel>
    {
        public SearchMovieQuery(string query, int page)
        {
            Query = query;
            Page = page;
        }

        public string Query { get; }

        public class Handler : IQueryHandler<SearchMovieQuery, PagedResultModel<MovieSearchResultModel>>
        {
            public Handler(TmdbClient client)
            {
                Client = client;
            }

            private TmdbClient Client { get; }

            public async Task<PagedResultModel<MovieSearchResultModel>> ExecuteAsync(SearchMovieQuery query, CancellationToken cancellationToken)
            {
                var page = query.Page ?? 1;

                var result = await Client.SearchMoviesAsync(query.Query, page, cancellationToken);

                return result.MapToModel(query.Size);
            }
        }
    }
}
