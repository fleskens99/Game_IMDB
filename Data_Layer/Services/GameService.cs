using DTOs;
using Entities;
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
            try
            {
                if (game is null) throw new ArgumentNullException(nameof(game));

                if (string.IsNullOrWhiteSpace(game.Name)) throw new ArgumentException("Title is required.", nameof(game.Name));

                if (string.IsNullOrWhiteSpace(game.Category)) throw new ArgumentException("Category is required.", nameof(game.Category));

                if (string.IsNullOrWhiteSpace(game.Description)) throw new ArgumentException("Description is required.", nameof(game.Description));

                game.Name = game.Name.Trim();
                game.Category = game.Category.Trim();
                game.Description = game.Description.Trim();

                return _repository.AddGame(game);
            }
            catch (ArgumentException)
            {
                throw; 
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the game.", ex);
            }
        }



        public List<GameDTO> GetGames()
        {
            try
            {
                var games = _repository.GetGames();

                if (games == null || games.Count == 0) throw new KeyNotFoundException("There are no games available.");

                return games;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve games due to a server or database error.", ex);
            }
        }


        public void EditGame(GameDTO game)
        {
            try
            {
                if (game is null) throw new ArgumentNullException(nameof(game));

                if (string.IsNullOrWhiteSpace(game.Name)) throw new ArgumentException("Title is required.", nameof(game.Name));

                if (string.IsNullOrWhiteSpace(game.Category)) throw new ArgumentException("Category is required.", nameof(game.Category));

                if (string.IsNullOrWhiteSpace(game.Description)) throw new ArgumentException("Description is required.", nameof(game.Description));

                game.Name = game.Name.Trim();
                game.Category = game.Category.Trim();
                game.Description = game.Description.Trim();

                _repository.EditGame(game);
            }
            catch (ArgumentException)
            {
                throw; 
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while editing the game.", ex);
            }
        }


        public void DeleteGame(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentException("Invalid game ID.", nameof(id));

                GameDTO? game = _repository.GetGameById(id);
                if (game == null) throw new KeyNotFoundException("Game does not exist or is already Deleted.");

                _repository.DeleteGame(id);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while Deleting the game.", ex);
            }
        }



        public GameDTO GetGameById(int id)
        {
            try
            {
                if (id <= 0) throw new ArgumentException("Invalid game ID.", nameof(id));

                GameDTO? game = _repository.GetGameById(id);

                if (game == null) throw new KeyNotFoundException("Game does not exist.");

                return game;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while Getting the game by Id.", ex);
            }
        }



        public byte[] GetImageBlob(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Invalid image ID.", nameof(id));

                byte[]? imageData = _repository.GetImageBlob(id);

                if (imageData == null || imageData.Length == 0) throw new KeyNotFoundException("No image found for this game.");
                return imageData;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while getting the image of the game.", ex);
            }
        }



        public bool CanEditGame(int gameId, int userId, bool isAdmin)
        {
            try
            {
                if (isAdmin) return true;
                if (gameId <= 0) throw new ArgumentException("Invalid game ID.", nameof(gameId));
                if (userId <= 0) throw new ArgumentException("Invalid user ID.", nameof(userId));

                var game = _repository.GetGameById(gameId);

                if (game == null) throw new KeyNotFoundException($"Game does not exist or is Deleted.");

                return game.CreatedByUserId == userId;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred to edit.", ex);
            }
        }

    }
}
