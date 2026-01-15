using DTOs;

namespace ViewModels
{
    public class GameIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] Picture { get; set; } = Array.Empty<byte>();
        public string Category { get; set; } = string.Empty;
        public double AverageRating { get; set; }
    }

    public class GameDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] Picture { get; set; } = Array.Empty<byte>();
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<RatingDTO>? Ratings { get; set; } 

    }
    public class AddGameViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[] Picture { get; set; } = Array.Empty<byte>();
        public int CreatedByUserId { get; set; }
    }

}
