using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> Get(Guid id, CancellationToken cancellationToken);
        Task Save(User user);
    }
}
