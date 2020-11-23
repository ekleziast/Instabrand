using System;

namespace Instabrand.Domain.UserRegistration
{
    public sealed class User
    {
        public Guid Id { get; }
        public string Email { get; }
        public string PasswordHash { get; }
        public State State { get; private set; }
        public DateTime CreateDate { get; }

        private Guid _concurrencyToken;
        
        private User() { }

        internal User(Guid id, string email, string passwordHash)
        {
            Id = id;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            State = State.Unconfirmed;
            CreateDate = DateTime.UtcNow;

            _concurrencyToken = Guid.NewGuid();
        }

        public void Confirm()
        {
            if (State != State.Unconfirmed)
                throw new InvalidOperationException($"Invalid state `{State}`");

            State = State.Confirmed;

            _concurrencyToken = Guid.NewGuid();
        }
    }
}
