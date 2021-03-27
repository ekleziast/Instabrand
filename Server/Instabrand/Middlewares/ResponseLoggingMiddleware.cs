using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Instabrand.Middlewares
{
    internal sealed class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseLoggingMiddleware> _logger;
        private readonly LogLevel _logLevel;

        public ResponseLoggingMiddleware(RequestDelegate next, LogLevel logLevel, ILogger<ResponseLoggingMiddleware> logger)
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

            // Т.к. стандартный Stream не поддерживает Seek, подменяем его на свой
            var bodyStream = context.Response.Body;
            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            // Выполняем запрос
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                //Internal server error
                _logger.LogError(exception.ToString());

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorResponse = JsonConvert.SerializeObject(new { requestId = context.TraceIdentifier });
                var errorResponseText = Encoding.UTF8.GetBytes(errorResponse);

                responseBodyStream.Write(errorResponseText, 0, errorResponseText.Length);
                responseBodyStream.Seek(0, SeekOrigin.Begin);

                await responseBodyStream.CopyToAsync(bodyStream);
                return;
            }

            var result = new StringBuilder();

            // Path + Status + Headers
            result.AppendLine($"RESPONSE {context.Request.Method} {context.Request.Path} {context.Request.Protocol}; STATUS: {context.Response.StatusCode}");
            foreach (var header in context.Request.Headers)
                result.Append(header.Key).Append(": ").AppendLine(string.Join("; ", header.Value));

            if (responseBodyStream.CanRead && responseBodyStream.Length > 0)
            {
                result.AppendLine("BODY:");

                responseBodyStream.Seek(0, SeekOrigin.Begin);

                var responseBody = new StreamReader(responseBodyStream).ReadToEnd();
                result.AppendLine(responseBody);

                _logger.Log(_logLevel, result.ToString());

                responseBodyStream.Seek(0, SeekOrigin.Begin);

                await responseBodyStream.CopyToAsync(bodyStream);
            }
        }
    }

    public static class ResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseLogging(this IApplicationBuilder app, LogLevel logLevel = LogLevel.Debug)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.UseMiddleware<ResponseLoggingMiddleware>(logLevel);
        }
    }
}
