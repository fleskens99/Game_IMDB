using DTOs;

namespace ViewModels
{
    public class GameWithRatingViewModel
    {
        public List<GameDTO> Games { get; set; } = new();
        public double AverageRating { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Picture { get; set; }
        public string Category { get; set; }

    }
}
