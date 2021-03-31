using Instabrand.Domain.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Authentication
{
    public sealed class UserGetter : IUserGetter
    {
        private readonly AuthenticationDbContext _context;

        public UserGetter(AuthenticationDbContext context)
        {
            _context = context;
        }

        public async Task<User> FindByEmail(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.SingleOrDefaultAsync(o => o.Email == email, cancellationToken);
        }

        public async Task<User> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users.SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
        }
    }
}
