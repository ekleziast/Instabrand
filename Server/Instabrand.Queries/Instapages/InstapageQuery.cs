using Instabrand.Shared.Infrastructure.CQRS;

namespace Instabrand.Queries.Instapages
{
    public sealed class InstapageQuery : IQuery<InstapageView>
    {
        public string InstagramLogin;

        public InstapageQuery(string instagramLogin)
        {
            InstagramLogin = instagramLogin;
        }
    }
}
