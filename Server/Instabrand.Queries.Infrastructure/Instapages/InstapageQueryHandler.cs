using Instabrand.Queries.Instapages;
using Instabrand.Shared.Infrastructure.CQRS;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Queries.Infrastructure.Instapages
{
    public sealed class InstapageQueryHandler : IQueryHandler<InstapageQuery, InstapageView>
    {
        private readonly InstapagesDbContext _context;

        public InstapageQueryHandler(InstapagesDbContext context)
        {
            _context = context;
        }

        public async Task<InstapageView> Handle(InstapageQuery query, CancellationToken cancellationToken)
        {
            var instapage = await _context.Instapages
                .AsNoTracking()
                .Include(o => o.Instaposts)
                .Where(o => o.InstagramLogin == query.InstagramLogin)
                .Select(o => new InstapageView
                {
                    Id = o.InstapageId,
                    State = o.State,
                    CreateDate = o.CreateDate,
                    Description = o.Description,
                    InstagramLogin = o.InstagramLogin,
                    Instaposts = o.Instaposts.Select(i => new InstapostView
                    {
                        Id = i.InstapostId,
                        Currency = i.Currency,
                        Description = i.Description,
                        Price = i.Price,
                        Timestamp = i.Timestamp,
                        Title = i.Title
                    }),
                    Telegram = o.Telegram,
                    Title = o.Title,
                    Vkontakte = o.Vkontakte
                })
                .SingleOrDefaultAsync(cancellationToken);

            return instapage;
        }
    }
}
