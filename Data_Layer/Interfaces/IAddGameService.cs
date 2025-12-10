using DTOs;

namespace Interfaces
{
    public interface IAddGameService
    {
        Task<int> AddGame(GameDTO game, CancellationToken cancellationToken = default);

    }

}
