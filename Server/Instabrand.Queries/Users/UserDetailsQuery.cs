using Instabrand.Shared.Infrastructure.CQRS;
using System;

namespace Instabrand.Queries.Users
{
    public sealed class UserDetailsQuery : IQuery<UserDetailsView>
    {
        public Guid UserId { get; }

        public UserDetailsQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
