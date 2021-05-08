using System;

namespace Instabrand.Queries.Users
{
    public sealed class UserDetailsView
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// <see cref="Email"/> state
        /// </summary>
        public EmailStateView EmailState { get; init; }

        /// <summary>
        /// User registration date
        /// </summary>
        public DateTime CreateDate { get; init; }
    }
}
