using System;
using System.Security.Claims;

namespace Instabrand.Extensions
{
    internal static class UserExtensions
    {
        public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.FindFirst(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value);
        }

        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(x => x.Type.Equals(ClaimTypes.Email))?.Value;
        }
    }
}
