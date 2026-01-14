using ViewModels;
using DTOs;

namespace VmMapper
{
    public class GameWithRatingVmMapper
    {
        public static GameWithRatingViewModel ToViewModel(GameDTO gameDto, double averageRating)
        {
            return new GameWithRatingViewModel
            {
                Games = new List<GameDTO> { gameDto },
                AverageRating = averageRating,
                Id = gameDto.Id,
                Name = gameDto.Name,
                Picture = gameDto.Picture,
                Category = gameDto.Category

            };
        }

    }
}
