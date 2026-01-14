using DTOs;
using Entities;

namespace Logic.EntityMapper
{
    public class RatingDTOMapper
    {
        public Rating Mapper(RatingDTO ratingDto)
        {
            if (ratingDto == null) throw new ArgumentNullException(nameof(ratingDto));
            return new Rating
            {
                id = ratingDto.Id,
                GameId = ratingDto.GameId,
                UserId = ratingDto.UserId,
                Comment = ratingDto.Comment,
                Score = ratingDto.Score
            };
        }
        public RatingDTO Mapper(Rating rating)
        {
            if (rating == null) throw new ArgumentNullException(nameof(rating));
            return new RatingDTO
            {
                Id = rating.id,
                GameId = rating.GameId,
                UserId = rating.UserId,
                Comment = rating.Comment,
                Score = rating.Score
            };
        }
    }
}
