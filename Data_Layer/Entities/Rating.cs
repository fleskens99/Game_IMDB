namespace Entities
{
    public class Rating
    {
        public int id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public Decimal Score { get; set; }
    }
}
