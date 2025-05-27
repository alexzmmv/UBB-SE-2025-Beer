namespace WinUiApp.Data.Data
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int DrinkId { get; set; }
        public Guid UserId { get; set; }
        public float? RatingValue { get; set; }
        public DateTime? RatingDate { get; set; }
        public bool? IsActive { get; set; }

        public User User { get; set; }
        public Drink Drink { get; set; }
    }
}
