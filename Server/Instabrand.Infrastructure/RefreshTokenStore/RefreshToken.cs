using System;

namespace Instabrand.Infrastructure.RefreshTokenStore
{
    internal sealed class RefreshToken
    {
        public string Id { get; }

        public Guid UserId { get; }

        public DateTime Expire { get; private set; }

        public RefreshToken(string id, Guid userId, TimeSpan expiresIn)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            UserId = userId;
            Expire = DateTime.UtcNow.Add(expiresIn);
        }

        private RefreshToken() { }

        public void Terminate()
        {
            Expire = DateTime.UtcNow;
        }
    }
}
