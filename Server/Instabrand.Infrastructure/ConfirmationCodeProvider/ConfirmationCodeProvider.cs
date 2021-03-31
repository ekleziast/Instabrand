using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Instabrand.Infrastructure.ConfirmationCodeProvider
{
    public sealed class ConfirmationCodeProvider : Domain.Registration.IConfirmationCodeProvider
    {
        private readonly int _lifetimeCodeInMinutes;
        private readonly SymmetricSecurityKey _securityKey;

        public ConfirmationCodeProvider(IOptions<ConfirmationCodeProviderOptions> options)
        {
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey));
            _lifetimeCodeInMinutes = options.Value.LifeTimeCodeInMinute;
        }

        public string Generate(string email)
        {
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email)
            });
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.Now.AddMinutes(_lifetimeCodeInMinutes),
                SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512Signature)
            };
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(jwtToken);
        }

        public bool Validate(in string token, out string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            email = null;
            try
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);
                if (jwtToken == null)
                    return false;
                var parameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = _securityKey
                };
                tokenHandler.ValidateToken(token, parameters, out _);
                email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
