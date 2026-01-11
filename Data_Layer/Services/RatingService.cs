using Interfaces;
using DTOs;
using System;

namespace Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepo _ratingRepo;

        public RatingService(IRatingRepo ratingRepo)
        {
            _ratingRepo = ratingRepo;
        }

        public int AddRating(RatingDTO rating)
        {
            if (rating == null)
                throw new ArgumentNullException(nameof(rating));

            if (_ratingRepo.UserHasRated(rating.UserId, rating.GameId))
                throw new InvalidOperationException("User has already rated this game.");

            if (rating.Score < 0 || rating.Score > 5)
                throw new ArgumentOutOfRangeException(nameof(rating.Score));

            if (string.IsNullOrWhiteSpace(rating.Comment))
                throw new ArgumentException("Comment is required");

            return _ratingRepo.AddRating(rating);
        }


    }
}
