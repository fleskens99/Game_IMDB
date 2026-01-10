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
                throw new ArgumentNullException(nameof(rating), "Rating is required");

            if (rating.Score < 0 || rating.Score > 5)
                throw new ArgumentOutOfRangeException(nameof(rating.Score), "Rating must be between 0 and 5");

            if (string.IsNullOrWhiteSpace(rating.Comment))
                throw new ArgumentException("Comment is required", nameof(rating.Comment));

            return _ratingRepo.AddRating(rating);
        }
    }
}
