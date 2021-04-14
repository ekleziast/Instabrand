using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Instabrand.Middlewares
{
    internal sealed class AspNetCoreEnvironmentPathBaseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly String _basePath;

        public AspNetCoreEnvironmentPathBaseMiddleware(RequestDelegate next, String basePath)
        {
            _next = next;
            _basePath = basePath;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.PathBase = _basePath;
            await _next(context);
        }
    }
}
