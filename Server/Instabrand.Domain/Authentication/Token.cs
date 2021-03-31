using System;

namespace Instabrand.Domain.Authentication
{
    public sealed class Token
    {
        public string AccessToken { get; }
        public TimeSpan ExpiresIn { get; }
        public string RefreshToken { get; }

        public Token(string accessToken, TimeSpan expiresIn, string refreshToken)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }
    }
}
