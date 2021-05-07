using System;

namespace Instabrand.Domain.Instapage
{
    public sealed class Instapage
    {
        public Guid Id { get; }

        public Guid? UserId { get; private set; }

        public string InstagramLogin { get; }
        public string InstagramId { get; }

        public string AccessToken { get; }

        public DateTime CreateDate { get; }
        public DateTime UpdateDate { get; private set; }

        public InstapageState State { get; private set; }

        private Instapage() { }
        public Instapage(Guid id, Guid userId, string instagramLogin, string instagramId, string accessToken)
        {
            Id = id;
            UserId = userId;
            InstagramLogin = instagramLogin ?? throw new ArgumentNullException(nameof(instagramLogin));
            InstagramId = instagramId ?? throw new ArgumentNullException(nameof(instagramId));
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            State = InstapageState.Created;
            CreateDate = DateTime.UtcNow;
            UpdateDate = CreateDate;
        }

        public void Disable()
        {
            switch (State)
            {
                case InstapageState.Created:
                case InstapageState.Enabled:
                    State = InstapageState.Disabled;
                    UpdateDate = DateTime.UtcNow;
                    break;
                case InstapageState.Disabled:
                    break;
            }
        }

        public void Enable()
        {
            if (UserId == null)
                throw new InvalidOperationException("That account has no owner!");

            switch (State)
            {
                case InstapageState.Enabled:
                    break;
                case InstapageState.Created:
                case InstapageState.Disabled:
                    State = InstapageState.Enabled;
                    UpdateDate = DateTime.UtcNow;
                    break;
            }
        }
    }
}
