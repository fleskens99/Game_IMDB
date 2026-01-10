
using DTOs;

namespace Interfaces
{
    public interface IRatingRepo
    {
        public int AddRating(RatingDTO Rating);
        public List<RatingDTO> GetRatings();
    }
}
