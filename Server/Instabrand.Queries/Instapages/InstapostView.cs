using System;

namespace Instabrand.Queries.Instapages
{
    public sealed class InstapostView
    {
        public string Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public string Currency { get; init; }
        public DateTime Timestamp { get; init; }
    }
}
