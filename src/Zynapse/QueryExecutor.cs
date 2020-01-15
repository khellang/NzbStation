using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Internal;

namespace Zynapse
{
    public class QueryExecutor : IQueryExecutor
    {
        public QueryExecutor(IServiceProvider provider)
        {
            Provider = provider;
        }

        private IServiceProvider Provider { get; }

        public Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            var queryType = query.GetType();

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult));

            var handler = Provider.GetService(handlerType);

            if (handler is null)
            {
                var displayName = TypeNameHelper.GetTypeDisplayName(queryType, fullName: false);
                throw new InvalidOperationException($"Could not find handler for query of type '{displayName}'.");
            }

            var method = handlerType.GetMethod(nameof(IQueryHandler<DummyQuery, int>.ExecuteAsync));

            return (Task<TResult>) method.Invoke(handler, new object[] { query, cancellationToken });
        }

        private class DummyQuery : IQuery<int>
        {
        }
    }
}
