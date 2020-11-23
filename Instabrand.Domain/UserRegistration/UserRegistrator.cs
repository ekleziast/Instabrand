using System;

namespace Instabrand.Domain.UserRegistration
{
    public sealed class UserRegistrator
    {
        private readonly IHasher _hasher;
        private readonly IConfirmationCodeProvider _confirmationCodeProvider;
        private readonly IConfirmationCodeSender _confirmationCodeSender;

        public UserRegistrator(IHasher hasher,
            IConfirmationCodeProvider confirmationCodeProvider,
            IConfirmationCodeSender confirmationCodeSender)
        {
            _hasher = hasher;
            _confirmationCodeProvider = confirmationCodeProvider;
            _confirmationCodeSender = confirmationCodeSender;
        }

        public User CreateUser(Guid id, string email, string password)
        {
            string passwordHash = _hasher.Hash(password);
            string confirmationCode = _confirmationCodeProvider.Generate(email);

            var user = new User(id, email, passwordHash);

            _confirmationCodeSender.Send(email, confirmationCode);

            return user;
        }
    }
}
