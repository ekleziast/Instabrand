using System;

namespace Instabrand.DatabaseMigrations.Entities
{
    internal sealed class User
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password hash
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// <see cref="Email"/> state
        /// </summary>
        public string EmailState { get; set; }

        /// <summary>
        /// User registration date
        /// </summary>
        public DateTime CreateDate { get; set; }

        public Guid ConcurrencyToken { get; set; }
    }
}
