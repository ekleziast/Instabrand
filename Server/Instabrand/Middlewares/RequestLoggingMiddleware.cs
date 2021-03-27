using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Instabrand.Middlewares
{
    internal sealed class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly LogLevel _logLevel;

        public RequestLoggingMiddleware(RequestDelegate next, LogLevel logLevel, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _logLevel = logLevel;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger", StringComparison.InvariantCultureIgnoreCase) ||
                context.Request.Path.StartsWithSegments("/hc", StringComparison.InvariantCultureIgnoreCase))
            {
                await _next(context);
                return;
            }

            var request = context.Request;

            var result = new StringBuilder();

            result.AppendLine($"REQUEST {request.Method} {request.Path} {request.Protocol}");

            foreach (var header in request.Headers)
                result.Append(header.Key).Append(": ").AppendLine(string.Join("; ", header.Value));

            if (request.ContentLength != null && request.ContentLength > 0 && request.ContentType.StartsWith("application/json"))
            {
                context.Request.EnableBuffering();

                result.AppendLine("BODY:");

                context.Request.Body.Position = 0;
                var content = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                result.AppendLine();
                result.AppendLine(content);
            }

            _logger.Log(_logLevel, result.ToString());

            await _next(context);
        }
    }

    internal static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, LogLevel logLevel = LogLevel.Debug)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<RequestLoggingMiddleware>(logLevel);
        }
    }
}
