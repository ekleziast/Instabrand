using Instabrand.Queries.Samples;
using Instabrand.Shared.Infrastructure.CQRS;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Queries.Infrastructure.Samples
{
    public sealed class SampleQueryHandler : IQueryHandler<SampleQuery, SampleView>
    {
        public async Task<SampleView> Handle(SampleQuery query, CancellationToken cancellationToken)
        {
            return new SampleView
            {
                Id = new Guid("180a5b1c-9877-4a3d-9737-6928a756ea55"),
                Title = "Test title"
            };
        }
    }
}
