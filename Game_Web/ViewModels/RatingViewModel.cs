namespace ViewModels
{
    public class RatingOnGameViewModel
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public decimal Score { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
