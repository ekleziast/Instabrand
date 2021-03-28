using System;

namespace Instabrand.Domain.Authentication
{
    public sealed class AccessToken
    {
        public string Value { get; }
        public TimeSpan ExpiresIn { get; }

        public AccessToken(string value, TimeSpan expiresIn)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            ExpiresIn = expiresIn;
        }
    }
}
