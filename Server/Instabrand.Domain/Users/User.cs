using System;

namespace Instabrand.Domain.Users
{
    public sealed class User
    {
        public Guid Id { get; }

        public DateTime CreateDate { get; }

        public string Email { get; }

        private User() { }
        public User(Guid id, DateTime createDate, string email)
        {
            Id = id;
            CreateDate = DateTime.UtcNow;
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
    }
}
