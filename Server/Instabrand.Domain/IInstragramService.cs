using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain
{
    public interface IInstragramService
    {
        /// <exception cref="ServiceUnavailableException" />
        Task<InstagramUser> GetUser(string username, CancellationToken cancellationToken);

        public sealed class ServiceUnavailableException : Exception { }
    }
}
