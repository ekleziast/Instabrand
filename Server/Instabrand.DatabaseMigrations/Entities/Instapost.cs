using System;

namespace Instabrand.DatabaseMigrations.Entities
{
    internal sealed class Instapost
    {
        public string InstapostId { get; set; }
        public Guid InstapageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}
