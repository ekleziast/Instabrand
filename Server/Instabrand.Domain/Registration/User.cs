using System;

namespace Instabrand.Domain.Registration
{
    public sealed class User
    {
        public Guid Id { get; }
        public string Email { get; }
        public string PasswordHash { get; }
        public DateTime CreateDate { get; }
        public EmailState EmailState { get; private set; }

        private Guid _concurrencyToken;

        private User() { }
        internal User(Guid id, string email, string passwordHash)
        {
            Id = id;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            CreateDate = DateTime.UtcNow;
            EmailState = EmailState.Unconfirmed;

            _concurrencyToken = Guid.NewGuid();
        }

        public void Confirm()
        {
            if (EmailState != EmailState.Unconfirmed)
                throw new UserEmailAlreadyConfirmedException();

            EmailState = EmailState.Confirmed;

            _concurrencyToken = Guid.NewGuid();
        }
    }
}
