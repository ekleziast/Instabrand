using Instabrand.Domain.Instapage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Instapages
{
    public sealed class InstapageRepository : IInstapageRepository
    {
        private readonly InstapagesDbContext _context;

        public InstapageRepository(InstapagesDbContext context)
        {
            _context = context;
        }

        public async Task<Instapage> Get(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Instapages.SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<Instapage> GetByInstagram(string instagram, CancellationToken cancellationToken)
        {
            return await _context.Instapages.SingleOrDefaultAsync(o => o.InstagramLogin == instagram, cancellationToken);
        }

        public async Task Save(Instapage instapage)
        {
            if (_context.Entry(instapage).State == EntityState.Detached)
                _context.Instapages.Add(instapage);

            await _context.SaveChangesAsync();
        }
    }
}
