using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain.Registration
{
    public interface IUserRepository
    {
        Task<User> FindByEmail(string email, CancellationToken cancellationToken);
        Task Save(User user);
    }
}
