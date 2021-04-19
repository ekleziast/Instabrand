using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Domain.Registration
{
    public sealed class UserRegistrationService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfirmationCodeProvider _confirmationCodeProvider;
        private readonly IConfirmationCodeSender _confirmationCodeSender;

        public UserRegistrationService(IPasswordHasher passwordHasher,
            IConfirmationCodeProvider confirmationCodeProvider,
            IConfirmationCodeSender confirmationCodeSender)
        {
            _passwordHasher = passwordHasher;
            _confirmationCodeProvider = confirmationCodeProvider;
            _confirmationCodeSender = confirmationCodeSender;
        }

        public async Task<User> CreateUser(string email, string password, CancellationToken cancellationToken)
        {
            var hash = _passwordHasher.HashPassword(password);

            var confirmationCode = _confirmationCodeProvider.Generate(email);
            await _confirmationCodeSender.SendConfirmationCode(email, confirmationCode, cancellationToken);

            return new User(Guid.NewGuid(), email, hash);
        }

        public async Task ResendConfirmationCode(User user, CancellationToken cancellationToken)
        {
            var confirmationCode = _confirmationCodeProvider.Generate(user.Email);
            await _confirmationCodeSender.SendConfirmationCode(user.Email, confirmationCode, cancellationToken);
        }

    }
}
