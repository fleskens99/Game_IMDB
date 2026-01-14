using DTOs;

namespace ViewModels
{
    public class GameWithRatingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Picture { get; set; }
        public string Category { get; set; }
        public double AverageRating { get; set; }
    }

}
