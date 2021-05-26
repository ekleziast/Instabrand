using System;

namespace Instabrand.Queries.Infrastructure.Instapages
{
    internal sealed class Instapost
    {
        public string InstapostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid InstapageId { get; set; }
    }
}
