using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain
{
    public interface IFileStorage
    {
        Stream Get(string fileName);
        Task SaveInstapostImage(string instapostId, string instagramLogin, Stream stream, CancellationToken cancellationToken);
    }
}
