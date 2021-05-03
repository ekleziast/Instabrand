using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Shared.Infrastructure.CQRS
{
    /// <summary>
    /// Запрос на получение данных
    /// </summary>
    /// <typeparam name="TResult">Тип возвращаемых данных</typeparam>
    public interface IQuery<TResult> { }

    /// <summary>
    /// Интерфейс для поиска всех <see cref="IQueryHandler{TQuery, TResult}"/> в сборке
    /// </summary>
    public interface IQueryHandler { }

    /// <summary>
    /// Интерфейс для обработки <see cref="IQuery{TResult}"/>
    /// </summary>
    public interface IQueryHandler<TQuery, TResult> : IQueryHandler where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Выполнение запроса
        /// </summary>
        /// <param name="query">Запрос</param>
        public Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
    }
}
