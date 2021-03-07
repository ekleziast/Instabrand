using Instabrand.Queries.Samples;
using Instabrand.Shared.Infrastructure.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Queries.Infrastructure.Samples
{
    public sealed class SampleListQueryHandler : IQueryHandler<SampleListQuery, Page<SampleListItemView>>
    {
        public async Task<Page<SampleListItemView>> HandleAsync(SampleListQuery query, CancellationToken cancellationToken)
        {
            var ownerId = Guid.NewGuid();

            var items = new List<SampleListItemView>
            {
                new SampleListItemView
                    {
                        Id = Guid.NewGuid(),
                        OwnerId = ownerId,
                        Title = "Test list item title"
                    },
                    new SampleListItemView
                    {
                        Id = Guid.NewGuid(),
                        OwnerId = ownerId,
                        Title = "Test list item title 2"
                    },
                    new SampleListItemView
                    {
                        Id = Guid.NewGuid(),
                        OwnerId = ownerId,
                        Title = "Test list item title 3"
                    }
            };

            return new Page<SampleListItemView>
            {
                Limit = query.Limit,
                Offset = query.Offset,
                Total = items.Count,
                Items = items
            };
        }
    }
}
