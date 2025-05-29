namespace WebApplication.Models
{
    public class UserPageViewModel
    {
        public required User CurrentUser { get; set; }
        public required IEnumerable<Review> CurrentUserReviews { get; set; }
        public required IEnumerable<string> CurrentUserDrinks { get; set; }
    }
}