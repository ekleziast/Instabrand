using Instabrand.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Users
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _context;

        public UserRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<User> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users
                .SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task Save(User user)
        {
            if (_context.Entry(user).State == EntityState.Detached)
                _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }
    }
}
