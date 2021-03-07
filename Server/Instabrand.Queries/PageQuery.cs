using Instabrand.Shared.Infrastructure.CQRS;

namespace Instabrand.Queries
{
    public abstract class PageQuery<T> : IQuery<Page<T>> where T : class
    {
        protected PageQuery(int offset, int limit)
        {
            Offset = offset;
            Limit = limit;
        }

        public int Limit { get; }

        public int Offset { get; }
    }
}
