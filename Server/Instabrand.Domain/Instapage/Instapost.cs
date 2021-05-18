using System;

namespace Instabrand.Domain.Instapage
{
    public sealed class Instapost
    {
        public string Id { get; }
        public Guid InstapageId { get; }
        public string Title { get; }
        public string Description { get; }
        public decimal Price { get; }
        public string Currency { get; }

        private Instapost() { }

        public Instapost(string id, Guid instapageId, string title, string description, decimal price, string currency)
        {
            Id = id;
            InstapageId = instapageId;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description;
            Price = price;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }
    }
}
