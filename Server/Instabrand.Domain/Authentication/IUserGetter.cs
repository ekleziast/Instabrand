using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain.Authentication
{
    public interface IUserGetter
    {
        Task<User> Get(Guid id, CancellationToken cancellationToken);
        Task<User> FindByEmail(string email, CancellationToken cancellationToken);
    }
}
