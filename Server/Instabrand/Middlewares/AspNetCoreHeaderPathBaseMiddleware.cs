using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Instabrand.Middlewares
{
    internal sealed class AspNetCoreHeaderPathBaseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly String _basePathKey;

        public AspNetCoreHeaderPathBaseMiddleware(RequestDelegate next, String basePathKey)
        {
            _next = next;
            _basePathKey = basePathKey;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(_basePathKey))
                context.Request.PathBase = new PathString(context.Request.Headers[_basePathKey].First());

            await _next(context);
        }
    }
}
