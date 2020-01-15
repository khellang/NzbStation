using System.Threading;
using System.Threading.Tasks;

namespace Zynapse
{
    public interface ICommandDispatcher
    {
        Task<TResult> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken);
    }
}
