using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.RefreshTokenStore
{
    public sealed class RefreshTokenStore : Domain.Authentication.IRefreshTokenStore
    {
        private readonly TimeSpan _lifeTime = TimeSpan.FromDays(1);
        private readonly RefreshTokenStoreDbContext _context;

        public RefreshTokenStore(RefreshTokenStoreDbContext context)
        {
            _context = context;
        }

        public async Task<string> Add(Guid userId, CancellationToken cancellationToken)
        {
            var refreshToken = new RefreshToken(Guid.NewGuid().ToString(), userId, _lifeTime);

            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync(cancellationToken);

            return refreshToken.Id;
        }

        public async Task ExpireAllTokens(Guid cameraOwnerId, CancellationToken cancellationToken)
        {
            var activeRefreshTokens = await _context.RefreshTokens
                .Where(o => o.UserId == cameraOwnerId)
                .Where(o => o.Expire > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            foreach (var activeRefreshToken in activeRefreshTokens)
                activeRefreshToken.Terminate();

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Domain.Authentication.RefreshToken> Reissue(string refreshToken, CancellationToken cancellationToken)
        {
            var oldRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(token =>
                token.Id == refreshToken && token.Expire > DateTime.UtcNow, cancellationToken);

            if (oldRefreshToken == null)
                return null;

            oldRefreshToken.Terminate();

            var newRefreshToken = new RefreshToken(Guid.NewGuid().ToString(), oldRefreshToken.UserId, _lifeTime);

            _context.RefreshTokens.Add(newRefreshToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new Domain.Authentication.RefreshToken(newRefreshToken.Id, newRefreshToken.UserId);
        }
    }
}
