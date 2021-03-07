using System;
using System.Collections.Generic;
using System.Linq;

namespace Instabrand.Shared.Infrastructure.CQRS
{
    internal class QueryHandlerRegistry : IQueryHandlerRegistry
    {
        /// <summary>
        /// Registered query handlers with keys <see cref="IQuery{TResult}"/> and their implementations <see cref="IQueryHandler{TQuery, TResult}"/>
        /// </summary>
        private readonly Dictionary<Type, Type> _queryHandlers = new();

        public IEnumerable<Type> Queries => _queryHandlers.Keys;
        public IEnumerable<Type> Handlers => _queryHandlers.Values;

        /// <summary>
        /// Register all query handlers in assembly
        /// </summary>
        /// <typeparam name="H">Handler from assembly to get assembly</typeparam>
        public void AddQueryHandlers<H>() where H : IQueryHandler
        {
            var handlers = typeof(H).Assembly.GetTypes()
                .Where(o => o.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var handler in handlers)
            {
                var query = handler
                    .GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                    .GetGenericArguments()
                    .First();

                _queryHandlers.Add(query, handler);
            }
        }

        public Type GetQueryHandler(Type queryType)
        {
            var handler = _queryHandlers.FirstOrDefault(o => o.Key.Name == queryType.Name).Value;
            if (handler == null)
                throw new KeyNotFoundException();

            return handler;
        }
    }
}
