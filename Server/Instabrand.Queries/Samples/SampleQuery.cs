using Instabrand.Shared.Infrastructure.CQRS;
using System;

namespace Instabrand.Queries.Samples
{
    public sealed class SampleQuery : IQuery<SampleView>
    {
        public Guid Id { get; }

        public SampleQuery(Guid id)
        {
            Id = id;
        }
    }
}
