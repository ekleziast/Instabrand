using System;

namespace Instabrand.Domain
{
    public sealed class InstagramUser
    {
        public string Username { get; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string AboutMe { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AvatarUrl { get; set; }

        public InstagramUser(string username)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
        }
    }
}
