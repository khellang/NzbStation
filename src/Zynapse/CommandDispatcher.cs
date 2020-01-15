using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;

namespace Zynapse
{
    public class CommandDispatcher : ICommandDispatcher
    {
        public CommandDispatcher(IServiceProvider provider)
        {
            Provider = provider;
        }

        private IServiceProvider Provider { get; }

        public Task<TResult> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
        {
            var commandType = command.GetType();

            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));

            var handler = Provider.GetService(handlerType);

            if (handler is null)
            {
                var displayName = TypeNameHelper.GetTypeDisplayName(commandType, fullName: false);
                throw new InvalidOperationException($"Could not find handler for command of type '{displayName}'.");
            }

            var method = handlerType.GetMethod(nameof(ICommandHandler<DummyCommand, int>.HandleAsync));

            return (Task<TResult>) method.Invoke(handler, new object[] { command, cancellationToken });
        }

        private class DummyCommand : ICommand<int>
        {
        }
    }
}
