using Instabrand.Domain.Registration;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Registration
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly RegistrationDbContext _context;

        public UserRepository(RegistrationDbContext context)
        {
            _context = context;
        }

        public async Task<User> FindByEmail(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.SingleOrDefaultAsync(co => co.Email == email, cancellationToken);
        }

        public async Task Save(User user)
        {
            if (_context.Entry(user).State == EntityState.Detached)
                _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }
    }
}
