using System;
using System.Threading;
using System.Threading.Tasks;

namespace NzbStation.Queries
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
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            var handler = Provider.GetService(handlerType);

            var method = handlerType.GetMethod(nameof(IQueryHandler<DummyQuery, int>.ExecuteAsync));

            return (Task<TResult>) method.Invoke(handler, new object[] { query, cancellationToken });
        }

        private class DummyQuery : IQuery<int> { }
    }
}