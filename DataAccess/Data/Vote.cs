namespace WinUiApp.Data.Data
{
    public class Vote
    {
        public int VoteId { get; set; }
        public Guid UserId { get; set; }
        public int DrinkId { get; set; }
        public DateTime VoteTime { get; set; }

        public User User { get; set; }
        public Drink Drink { get; set; }
    }
}
