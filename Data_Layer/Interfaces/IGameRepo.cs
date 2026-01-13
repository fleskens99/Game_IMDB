using DTOs;

namespace Interfaces
{
    public interface IGameRepo
    {
        int AddGame(GameDTO game);
        List<GameDTO> GetGames();
        public void EditGame(GameDTO game);
        public void DeleteGame(int id);
        GameDTO GetGameById(int id);
        byte[]? GetImageBlob(int id);
    }
}
