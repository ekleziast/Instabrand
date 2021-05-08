using Instabrand.Queries.Users;
using System;

namespace Instabrand.Queries.Infrastructure.Users
{
    internal sealed class User
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public EmailStateView EmailState { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
