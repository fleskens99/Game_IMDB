namespace DTOs
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public decimal Score { get; set; }
    }
}
