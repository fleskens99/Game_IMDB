using DTOs;
using Interfaces;
using static System.Formats.Asn1.AsnWriter;

namespace Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepo _ratingRepo;
        public int AverageRating { get; private set; }

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

        public List<RatingDTO> GetRatingsByGame(int gameId)
        {
            return _ratingRepo.GetRatingsByGame(gameId);
        }

        public bool UserHasRated(int userId, int gameId)
        {
            return _ratingRepo.UserHasRated(userId, gameId);
        }

        public int GetAverageScoreFromGames(int gameId)
        {
            var scores = _ratingRepo.GetAverageScoreFromGames(gameId);

            if (scores == null || scores.Count == 0) return 0;

            return (int)scores.Average();
        }


    }
}
