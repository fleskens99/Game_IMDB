using DTOs;

namespace Interfaces
{
    public interface IGameService
    {
        int AddGame(GameDTO game);
        public List<GameDTO> GetGames();
        public void EditGame(GameDTO game);
        public void DeleteGame(int id);
        public GameDTO GetGameById(int id);
        public byte[]? GetImageBlob(int id);
        public bool CanEditGame(int gameId, int userId, bool isAdmin);

    }

}
