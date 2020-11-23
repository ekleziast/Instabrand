using Microsoft.AspNetCore.Identity;

namespace Instabrand.Infrastructure.Passwords
{
    public sealed class PasswordHasher : Domain.UserRegistration.IHasher
    {
        private sealed class User { }

        private readonly IPasswordHasher<User> _hasher = new PasswordHasher<User>();

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(new User(), hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }

        public string Hash(string password)
        {
            return _hasher.HashPassword(new User(), password);
        }
    }
}
