using ViewModels;
using DTOs;

namespace VmMapper
{
    public class GameEntityToVm
    {
        public GameVM Mappper(GameDTO game)
        {
            return new GameVM
            {
                Id = game.Id,
                Name = game.Name,
                Category = game.Category,
                Description = game.Description,
                Picture = game.Picture
            };
        }

        public GameDTO Mappper(GameVM game)
        {
            return new GameDTO
            {
                Id = game.Id,
                Name = game.Name,
                Category = game.Category,
                Description = game.Description,
                Picture = game.Picture
            };
        }
    }
}
