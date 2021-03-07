using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Shared.Infrastructure.CQRS
{
    public interface IQueryProcessor
    {
        /// <summary>
        /// Обработка запроса <see cref="IQuery{TResult}"/> через соответствующий <see cref="IQueryHandler{TQuery, TResult}"/>
        /// </summary>
        Task<TResult> Process<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
    }
}
