using Instabrand.Domain.UserRegistration;
using System;

namespace Instabrand.Infrastructure.UserRegistration
{
    public sealed class ConfirmationCodeSender : IConfirmationCodeSender
    {
        public void Send(string email, string confirmationCode)
        {
            throw new NotImplementedException();
        }
    }
}
