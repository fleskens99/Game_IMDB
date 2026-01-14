
using DTOs;

namespace Interfaces
{
    public interface IRatingRepo
    {
        public int AddRating(RatingDTO Rating);
        public List<RatingDTO> GetRatingsByGame(int gameId);
        public bool UserHasRated(int userId, int gameId);
        public List<decimal> GetAverageScoreFromGames(int gameId);
    }
}
