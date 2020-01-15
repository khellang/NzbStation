using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NzbStation.Data;
using NzbStation.Extensions;
using NzbStation.Models;
using Zynapse;

namespace NzbStation.Queries
{
    public class GetAllMoviesQuery : PagedQuery<MovieDetailsModel>
    {
        public GetAllMoviesQuery(int? page, int? size)
        {
            Page = page;
            Size = size;
        }

        public class Handler : IQueryHandler<GetAllMoviesQuery, PagedResultModel<MovieDetailsModel>>
        {
            public Handler(Database database)
            {
                Database = database;
            }

            private Database Database { get; }

            public Task<PagedResultModel<MovieDetailsModel>> ExecuteAsync(GetAllMoviesQuery query, CancellationToken cancellationToken)
            {
                return Database.Movies.Select(x => x.MapToModel()).AsPagedAsync(query, cancellationToken);
            }
        }
    }
}
