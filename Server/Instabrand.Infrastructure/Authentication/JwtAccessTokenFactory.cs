using Instabrand.Domain.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Authentication
{
    public sealed class JwtAccessTokenFactory : IAccessTokenFactory
    {
        private readonly JwtAuthOptions _authOptions;

        public JwtAccessTokenFactory(IOptions<JwtAuthOptions> authOptions)
        {
            _authOptions = authOptions.Value;
        }

        public Task<AccessToken> Create(User user, CancellationToken cancellationToken)
        {
            var jwtSecurityToken = new JwtSecurityToken(
                   _authOptions.Issuer,
                   _authOptions.Audience,
                   new ClaimsIdentity(new List<Claim>
                   {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                   }, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType).Claims,
                   DateTime.UtcNow,
                   DateTime.UtcNow.AddMinutes(_authOptions.LifeTimeMinutes),
                   new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecretKey)), SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Task.FromResult(new AccessToken(token, TimeSpan.FromMinutes(_authOptions.LifeTimeMinutes)));
        }
    }
}
