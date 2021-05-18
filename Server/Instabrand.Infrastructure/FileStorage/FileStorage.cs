using Instabrand.Domain;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.FileStorage
{
    public sealed class FileStorage : IFileStorage
    {
        private readonly string _filePath;

        public FileStorage(IOptions<FilePathOptions> options)
        {
            _filePath = options.Value.FilePath;
        }

        public Stream Get(string filename)
        {
            var filePath = Path.Combine(_filePath, filename);

            if (!File.Exists(filePath))
                throw new InvalidOperationException("File does not exists");

            var fs = new FileStream(filePath, FileMode.Open);

            return fs;
        }

        public async Task Save(string filename, string instagramLogin, Stream stream, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);

            var filePath = Path.Combine(_filePath, instagramLogin);

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            filePath = Path.Combine(filePath, filename);

            using var fs = new FileStream(filePath, FileMode.Create);

            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(fs, cancellationToken);
        }
    }
}
