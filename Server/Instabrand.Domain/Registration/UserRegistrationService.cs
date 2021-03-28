using System;

namespace Instabrand.Domain.Registration
{
    public sealed class UserRegistrationService
    {
        private readonly IPasswordHasher _passwordHasher;

        public UserRegistrationService(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public User CreateUser(string email, string password)
        {
            var hash = _passwordHasher.HashPassword(password);

            return new User(Guid.NewGuid(), email, hash);
        }
    }
}
