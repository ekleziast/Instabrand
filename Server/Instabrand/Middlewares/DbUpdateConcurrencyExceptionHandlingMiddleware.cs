using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Instabrand.Middlewares
{
    public sealed class DbUpdateConcurrencyExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public DbUpdateConcurrencyExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILoggerFactory loggerFactory)
        {
            do
            {
                try
                {
                    await _next(context);
                    break;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var logger = loggerFactory
                        .CreateLogger<DbUpdateConcurrencyExceptionHandlingMiddleware>();

                    if (context.Request.Body != null && context.Request.Body.CanSeek)
                        context.Request.Body.Seek(0, SeekOrigin.Begin);

                    logger.LogWarning($"Some concurrent query had time to update the state. Retry handling request process for {context.Request.Method} {context.Request.Path}:{context.TraceIdentifier}");

                    // Recreate scope for use new db context
                    context.RequestServices = ServiceProviderServiceExtensions.CreateScope(context.RequestServices).ServiceProvider;

                    continue;
                }
            } while (true);
        }
    }
    internal static class DbUpdateConcurrencyExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseDbConcurrencyExceptionHandling(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<DbUpdateConcurrencyExceptionHandlingMiddleware>();
        }
    }
}