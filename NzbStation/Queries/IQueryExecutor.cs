using System.Threading;
using System.Threading.Tasks;

namespace NzbStation.Queries
{
    public interface IQueryExecutor
    {
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
    }
}