using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Shared.Infrastructure.CQRS
{
    public interface ICommand { }

    /// <summary>
    /// Интерфейс для обработки <see cref="ICommand"/>
    /// </summary>
    public interface ICommandHandler<TCommand> where TCommand: ICommand
    {
        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="command">Команда</param>
        Task HandleAsync(TCommand command, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Интерфейс для поиска всех <see cref="ICommandHandler{TCommand}"/> в сборке
    /// </summary>
    public interface ICommandHandler { }
}
