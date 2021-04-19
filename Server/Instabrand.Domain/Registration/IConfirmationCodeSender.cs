using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain.Registration
{
    public interface IConfirmationCodeSender
    {
        Task SendConfirmationCode(string email, string confirmationCode, CancellationToken cancellationToken);
    }
}
