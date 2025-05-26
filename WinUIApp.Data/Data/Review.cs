using System;

namespace WinUiApp.Data.Data
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? IsActive { get; set; }

        public Rating Rating { get; set; }
        public User User { get; set; }
    }
}
