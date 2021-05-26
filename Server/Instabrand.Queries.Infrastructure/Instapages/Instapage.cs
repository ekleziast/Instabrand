using Instabrand.Queries.Instapages;
using System;
using System.Collections.Generic;

namespace Instabrand.Queries.Infrastructure.Instapages
{
    internal sealed class Instapage
    {
        public Guid InstapageId { get; set; }
        public string InstagramLogin { get; set; }
        public string Vkontakte { get; set; }
        public string Telegram { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public InstapageState State { get; set; }
        public IEnumerable<Instapost> Instaposts { get; set; }
    }
}
