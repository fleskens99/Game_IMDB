using DTOs;
using Entities;


namespace Mapper
{
    public class GameDTOToEntity
    {
        public Game Mapper(GameDTO gameDto)
        {
            if (gameDto == null) throw new ArgumentNullException(nameof(gameDto));
            return new Game
            {
                Id = gameDto.Id,
                Name = gameDto.Name,
                Category = gameDto.Category,
                Description = gameDto.Description,
                Picture = gameDto.Picture
            };
        }
        public GameDTO Mapper(Game game) 
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
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
