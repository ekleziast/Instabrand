using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Threading.Tasks;

namespace Instabrand.Middlewares
{
    internal sealed class RequestIdMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _contextAccessor;

        public RequestIdMiddleware(RequestDelegate next, IHttpContextAccessor contextAccessor, ILogger<RequestIdMiddleware> logger)
        {
            _next = next;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = context.Request.Headers["X-Request-Id"];

            if (!string.IsNullOrWhiteSpace(requestId))
            {
                context.TraceIdentifier = requestId;
                _contextAccessor.HttpContext ??= context;
            }

            using (_logger.BeginScope("#" + context.TraceIdentifier))
            {
                LogContext.PushProperty("TraceIdentifier", context.TraceIdentifier);
                await _next(context);
            }
        }
    }

    internal static class RequestIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestId(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestIdMiddleware>();
        }
    }
}
