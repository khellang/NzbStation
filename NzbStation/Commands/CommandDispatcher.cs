using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace NzbStation.Commands
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
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));

            var handler = Provider.GetRequiredService(handlerType);

            var method = handlerType.GetMethod(nameof(ICommandHandler<DummyCommand, int>.HandleAsync));

            return (Task<TResult>) method.Invoke(handler, new object[] { command, cancellationToken });
        }

        private class DummyCommand : ICommand<int> { }
    }
}