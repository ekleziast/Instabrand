using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain
{
    public interface IFileStorage
    {
        Stream Get(string filename);
        Task Save(string filename, string instagramLogin, Stream stream, CancellationToken cancellationToken);
    }
}
