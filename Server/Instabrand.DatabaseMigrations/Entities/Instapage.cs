using System;

namespace Instabrand.DatabaseMigrations.Entities
{
    internal sealed class Instapage
    {
        public Guid InstapageId { get; set; }

        public Guid? UserId { get; set; }

        public string InstagramLogin { get; set; }
        public string InstagramId { get; set; }

        public string AccessToken { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string State { get; set; }

        public Guid ConcurrencyToken { get; set; }
    }
}
