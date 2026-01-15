using DTOs;
using Entities;
using ViewModels;

namespace VmMapper
{
    public class GameVmMapper
    {
        public static GameIndexViewModel ToIndexViewModel(GameDTO gameDto, double averageRating)
        {
            return new GameIndexViewModel
            {
                Id = gameDto.Id,
                Name = gameDto.Name,
                Picture = gameDto.Picture ?? Array.Empty<byte>(),
                Category = gameDto.Category,
                AverageRating = averageRating
            };
        }

        public static GameDetailsViewModel ToDetailsViewModel(GameDTO gameDto, List<RatingDTO> ratings)
        {
            return new GameDetailsViewModel
            {
                Id = gameDto.Id,
                Name = gameDto.Name,
                Picture = gameDto.Picture ?? Array.Empty<byte>(),
                Category = gameDto.Category,
                Description = gameDto.Description,
                Ratings = ratings
            };
        }

        public static GameDTO ToAddGameDTO(AddGameViewModel gameVm)
        {
            return new GameDTO
            {
                Name = gameVm.Name,
                Category = gameVm.Category,
                Description = gameVm.Description,
                Picture = gameVm.Picture,
                CreatedByUserId = gameVm.CreatedByUserId
            };
        }

        public static AddGameViewModel ToAddGameViewModel(GameDTO gameDto)
        {
            return new AddGameViewModel
            {
                Name = gameDto.Name,
                Category = gameDto.Category,
                Description = gameDto.Description,
                Picture = gameDto.Picture,
                CreatedByUserId = gameDto.CreatedByUserId

            };
        }
    }
}
