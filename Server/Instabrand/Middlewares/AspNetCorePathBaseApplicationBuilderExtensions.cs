using System;
using Microsoft.AspNetCore.Builder;

namespace Instabrand.Middlewares
{
    internal static class AspNetCorePathBaseApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAspNetCorePathBase(this IApplicationBuilder app, String basePathKey = "ASPNETCORE-PATH-BASE")
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (String.IsNullOrEmpty(basePathKey)) throw new ArgumentNullException(nameof(basePathKey));

            var basePath = Environment.GetEnvironmentVariable(basePathKey);
            if (!String.IsNullOrEmpty(basePath))
            {
                app.UseMiddleware<AspNetCoreEnvironmentPathBaseMiddleware>(
                    typeof(AspNetCoreEnvironmentPathBaseMiddleware), basePath);
            }

            return app.UseMiddleware<AspNetCoreHeaderPathBaseMiddleware>(basePathKey);
        }
    }
}
