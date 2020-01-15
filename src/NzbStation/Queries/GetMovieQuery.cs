using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NzbStation.Data;
using NzbStation.Models;
using Zynapse;

namespace NzbStation.Queries
{
    public class GetMovieQuery : IQuery<MovieDetailsModel>
    {
        public GetMovieQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public class Handler : IQueryHandler<GetMovieQuery, MovieDetailsModel>
        {
            public Handler(Database database)
            {
                Database = database;
            }

            private Database Database { get; }

            public async Task<MovieDetailsModel> ExecuteAsync(GetMovieQuery query, CancellationToken cancellationToken)
            {
                return (await Database.Movies.FirstOrDefaultAsync(x => x.Id.Equals(query.Id), cancellationToken))?.MapToModel();
            }
        }
    }
}
