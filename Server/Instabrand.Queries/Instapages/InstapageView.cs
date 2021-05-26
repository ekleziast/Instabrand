using System;
using System.Collections.Generic;

namespace Instabrand.Queries.Instapages
{
    public sealed class InstapageView
    {
        public Guid Id { get; init; }
        public string InstagramLogin { get; init; }
        public string Vkontakte { get; init; }
        public string Telegram { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime CreateDate { get; init; }
        public InstapageState State { get; init; }
        public IEnumerable<InstapostView> Instaposts { get; init; }
    }
}
