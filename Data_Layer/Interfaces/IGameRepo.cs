using DTOs;

namespace Interfaces
{
    public interface IGameRepo
    {
        List<GameDTO> GetGames();
        Task<int> AddGame(GameDTO game, CancellationToken cancellationToken = default);
        GameDTO GetGameById(int id);
        byte[]? GetImageBlob(int id);
    }
}
