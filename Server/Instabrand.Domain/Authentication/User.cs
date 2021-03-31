using System;

namespace Instabrand.Domain.Authentication
{
    public sealed class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public EmailState EmailState { get; private set; }

        private User() { }
    }
}
