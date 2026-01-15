using ViewModels;
using DTOs;

namespace VmMapper
{
    public class RatingOnGameVmMapper
    {
        public static RatingOnGameViewModel RatingDetailsViewModel(RatingDTO rating)
        {
            return new RatingOnGameViewModel
            {
                Id = rating.Id,
                GameId = rating.GameId,
                UserId = rating.UserId,
                Comment = rating.Comment,
                Score = rating.Score,
            };
        }

        public static RatingDTO RatingDetailsDTO(RatingOnGameViewModel ratingVm)
        {
            return new RatingDTO
            {
                Id = ratingVm.Id,
                GameId = ratingVm.GameId,
                UserId = ratingVm.UserId,
                Comment = ratingVm.Comment,
                Score = ratingVm.Score,
            };
        }
    }
}
