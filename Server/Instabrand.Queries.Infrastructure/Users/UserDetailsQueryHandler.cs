using Instabrand.Queries.Users;
using Instabrand.Shared.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Queries.Infrastructure.Users
{
    public sealed class UserDetailsQueryHandler : IQueryHandler<UserDetailsQuery, UserDetailsView>
    {
        private readonly UsersDbContext _context;

        public UserDetailsQueryHandler(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<UserDetailsView> Handle(UserDetailsQuery query, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(o => o.UserId == query.UserId)
                .Select(o => new UserDetailsView
                {
                    Id = o.UserId,
                    EmailState = o.EmailState,
                    CreateDate = o.CreateDate,
                    Email = o.Email
                })
                .SingleOrDefaultAsync(cancellationToken);

            return user;
        }
    }
}
