using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Shared.Infrastructure.CQRS
{
    internal sealed class QueryProcessor : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IQueryHandlerRegistry _registry;

        public QueryProcessor(IServiceProvider serviceProvider, IQueryHandlerRegistry queryHandlerRegistry)
        {
            _serviceProvider = serviceProvider;
            _registry = queryHandlerRegistry;
        }

        public async Task<TResult> Process<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            Type handlerType = _registry.GetQueryHandler(query.GetType());
            dynamic handler = _serviceProvider.GetService(handlerType);

            return await handler.Handle((dynamic)query, cancellationToken);
        }
    }
}
