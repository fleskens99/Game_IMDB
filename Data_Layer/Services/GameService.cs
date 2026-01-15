using DTOs;
using Interfaces;

namespace Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepo _repository;

        public GameService(IGameRepo repository)
        {
            _repository = repository;
        }

        public int AddGame(GameDTO game)
        {
            if (game is null) throw new ArgumentNullException(nameof(game));
            if (string.IsNullOrWhiteSpace(game.Name)) throw new ArgumentException("Title is required.", nameof(game.Name));
            if (string.IsNullOrWhiteSpace(game.Category)) throw new ArgumentException("Category is required.", nameof(game.Category));

            game.Name = game.Name.Trim();
            game.Category = game.Category.Trim();
            game.Description = game.Description?.Trim();

            var newId = _repository.AddGame(game);
            return newId;
        }

        public List<GameDTO> GetGames()
        {
            return _repository.GetGames();
        }

        public void EditGame(GameDTO game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            if (string.IsNullOrWhiteSpace(game.Name)) throw new ArgumentException("Name is required", nameof(game.Name));

            game.Name = game.Name.Trim();
            game.Category = game.Category.Trim();
            game.Description = game.Description?.Trim();

            _repository.EditGame(game);
        }

        public void DeleteGame(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid game ID.", nameof(id));
            _repository.DeleteGame(id);
        }

        public GameDTO? GetGameById(int id)
        {
            return _repository.GetGameById(id);
        }

        public byte[]? GetImageBlob(int id)
        {
            return _repository.GetImageBlob(id);
        }

        public bool CanEditGame(int gameId, int userId, bool isAdmin)
        {
            if (isAdmin) return true;
            if (userId <= 0) return false;
            var game = _repository.GetGameById(gameId);
            return game != null && game.CreatedByUserId == userId;
        }
    }
}
