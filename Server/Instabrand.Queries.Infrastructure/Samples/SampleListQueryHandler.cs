using Instabrand.Queries.Samples;
using Instabrand.Shared.Infrastructure.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Queries.Infrastructure.Samples
{
    public sealed class SampleListQueryHandler : IQueryHandler<SampleListQuery, Page<SampleListItemView>>
    {
        public async Task<Page<SampleListItemView>> Handle(SampleListQuery query, CancellationToken cancellationToken)
        {
            var ownerId = new Guid("f8504337-b7e1-433a-8d0f-cbfedbe879bc");

            var testData = new List<SampleListItemView>
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

            var items = testData
                .Where(o => o.OwnerId == query.OwnerId)
                .AsQueryable();

            return new Page<SampleListItemView>
            {
                Limit = query.Limit,
                Offset = query.Offset,
                Total = items.Count(),
                Items = items
                    .Skip(query.Offset)
                    .Take(query.Limit)
            };
        }
    }
}
