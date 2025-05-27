using System.ComponentModel.DataAnnotations;
using WinUIApp.ProxyServices.Models;

namespace WinUIApp.WebUI.Models
{    public class RatingReviewViewModel
    {
        public RatingReviewViewModel()
        {
            // No need to initialize ReviewContent since it's nullable
        }
        
        public int DrinkId { get; set; }
        
        // Optional: If this is set, we're adding a review to an existing rating
        public int? RatingId { get; set; }
        
        [Required(ErrorMessage = "Rating score is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Score { get; set; }
          [StringLength(500, ErrorMessage = "Review content cannot exceed 500 characters")]
        public string? ReviewContent { get; set; }
        
        // Default user ID for simplicity (in real app would come from authentication)
        public Guid UserId { get; set; } = new Guid(); // emtpy guid for now
    }
}
