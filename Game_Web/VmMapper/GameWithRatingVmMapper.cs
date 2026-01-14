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
                Id = gameDto.Id,
                Name = gameDto.Name,
                Picture = gameDto.Picture,
                Category = gameDto.Category,
                AverageRating = averageRating
            };
        }


    }
}
