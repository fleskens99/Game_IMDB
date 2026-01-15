using DTOs;

namespace Interfaces
{
    public interface IRatingService
    {
        public int AddRating(RatingDTO Rating);
        public List<RatingDTO> GetRatingsByGame(int gameId);
        public bool UserHasRated(int userId, int gameId);
        public double GetAverageScoreFromGames(int gameId);

    }
}
