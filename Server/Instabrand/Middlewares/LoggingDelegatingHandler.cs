using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Middlewares
{
    internal sealed class LoggingDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var scheme = request.RequestUri.Scheme.ToUpper();

            await MakeRequestLog(request, scheme);

            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                await MakeResponseLog(response, scheme);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, "Http request error");
                throw;
            }
        }

        private async Task MakeRequestLog(HttpRequestMessage request, string scheme)
        {
            var result = new StringBuilder();

            result.AppendLine($"REQUEST {request.Method} {request.RequestUri} {scheme}/{request.Version}");

            foreach (var header in request.Headers.Concat(request.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>()))
                result.Append(header.Key).Append(": ").AppendLine(string.Join("; ", header.Value));

            if (request.Content != null)
            {
                var content = Equals(request.Content.Headers.ContentType, MediaTypeHeaderValue.Parse("multipart/form-data"))
                    ? "FILE"
                    : await request.Content.ReadAsStringAsync();

                result.AppendLine();
                result.AppendLine(content);
            }

            _logger.LogInformation(result.ToString());
        }

        private async Task MakeResponseLog(HttpResponseMessage response, string scheme)
        {
            var result = new StringBuilder();

            result.AppendLine($"RESPONSE {scheme}/{response.Version} {(int)response.StatusCode} {response.ReasonPhrase}");

            foreach (var header in response.Headers.Concat(response.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>()))
                result.Append(header.Key).Append(": ").AppendLine(string.Join("; ", header.Value));

            result.AppendLine();

            var supportedTypes = new[] { "text/plain", "application/json" };

            if (response.Content?.Headers?.ContentType != null)
            {
                var content = supportedTypes.Contains(response.Content.Headers.ContentType.MediaType)
                    ? await response.Content.ReadAsStringAsync()
                    : $"Content-Type: {response.Content.Headers.ContentType}; Content-Length: {response.Content.Headers.ContentLength}";

                result.AppendLine(content);
            }

            _logger.LogInformation(result.ToString());
        }
    }

    public static class LoggingDelegatingHandlerExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app,
            LogLevel requestLogLevel = LogLevel.Debug,
            LogLevel responseLogLevel = LogLevel.Debug)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            return app.UseRequestLogging(requestLogLevel).UseResponseLogging(responseLogLevel);
        }
    }
}
