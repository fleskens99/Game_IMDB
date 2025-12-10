using DTOs;
using Interfaces;

namespace Services
{
    public class AddGameService : IAddGameService
    {
        private readonly IGameRepo _repository;

        public AddGameService(IGameRepo repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<int> AddGame(GameDTO game, CancellationToken cancellationToken = default)
        {
            if (game is null) throw new ArgumentNullException(nameof(game));
            if (string.IsNullOrWhiteSpace(game.Name)) throw new ArgumentException("Title is required.", nameof(game.Name));
            if (string.IsNullOrWhiteSpace(game.Category)) throw new ArgumentException("Category is required.", nameof(game.Category));

            // Business rules, normalization
            game.Name = game.Name.Trim();
            game.Category = game.Category.Trim();
            game.Description = game.Description?.Trim();

               // Delegate to repository (both are in DAL)
                var newId = await _repository.AddGame(game, cancellationToken).ConfigureAwait(false);
            return newId;
        }
    }
}
