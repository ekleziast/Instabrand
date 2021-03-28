using System;

namespace Instabrand.Domain.Authentication
{
    public sealed class RefreshToken
    {
        public string Value { get; }
        public Guid UserId { get; }

        public RefreshToken(string value, Guid userId)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            UserId = userId;
        }
    }
}
