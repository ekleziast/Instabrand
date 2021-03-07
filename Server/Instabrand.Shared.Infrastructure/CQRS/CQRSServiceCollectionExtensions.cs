using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Instabrand.Shared.Infrastructure.CQRS
{
    public static class CQRSServiceCollectionExtensions
    {
        /// <summary>
        /// Register all query handlers in assembly
        /// </summary>
        /// <typeparam name="H">Handler from assembly to get assembly</typeparam>
        public static void AddQueryHandlers<H>(this IServiceCollection services) where H : IQueryHandler
        {
            var handlers = typeof(H).Assembly.GetTypes()
                .Where(o => o.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            handlers.ToList().ForEach(o => System.Diagnostics.Debug.WriteLine(o.Name));

            foreach (var handler in handlers)
            {
                services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)), handler);
            }
        }

        /// <summary>
        /// Register all command handlers in assembly
        /// </summary>
        /// <typeparam name="H">Handler from assembly to get assembly</typeparam>
        public static void AddCommandHandlers<H>(this IServiceCollection services) where H: ICommandHandler
        {
            var handlers = typeof(H).Assembly.GetTypes()
                .Where(o => o.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var handler in handlers)
            {
                services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)), handler);
            }
        }
    }
}
