using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain.Instapage
{
    public interface IInstapageRepository
    {
        Task<Instapage> Get(Guid id, CancellationToken cancellationToken);
        Task<Instapage> GetByInstagram(string instagram, CancellationToken cancellationToken);
        Task Save(Instapage instapage);
    }
}
