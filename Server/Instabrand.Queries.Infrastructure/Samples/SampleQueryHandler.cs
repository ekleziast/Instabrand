using Instabrand.Queries.Samples;
using Instabrand.Shared.Infrastructure.CQRS;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Queries.Infrastructure.Samples
{
    public sealed class SampleQueryHandler : IQueryHandler<SampleQuery, SampleView>
    {
        public async Task<SampleView> HandleAsync(SampleQuery query, CancellationToken cancellationToken)
        {
            return new SampleView
            {
                Id = Guid.NewGuid(),
                Title = "Test title"
            };
        }
    }
}
