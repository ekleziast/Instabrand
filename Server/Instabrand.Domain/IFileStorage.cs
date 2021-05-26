using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain
{
    public interface IFileStorage
    {
        (Stream stream, string extension) GetFile(string filename, string instagramLogin);
        (Stream stream, string extension) GetFileWithoutExtension(string filename, string instagramLogin);
        Task Save(string filename, string instagramLogin, Stream stream, CancellationToken cancellationToken);
    }
}
