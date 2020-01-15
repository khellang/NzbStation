using System.Threading;
using System.Threading.Tasks;

namespace Zynapse
{
    public interface IQueryExecutor
    {
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
    }
}
