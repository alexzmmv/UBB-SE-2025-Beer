namespace WebApplication.Models
{
    public class AppealDetailsViewModel
    {
        public required User User { get; set; }
        public required IEnumerable<Review> Reviews { get; set; }
    }
}