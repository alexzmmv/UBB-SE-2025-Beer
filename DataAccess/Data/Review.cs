namespace WinUiApp.Data.Data
{
    public class Review
    {
        //public Review()
        //{
        //    this.Content = string.Empty;
        //}

        //public Review(int reviewId, Guid userId, Rating rating, string content, DateTime createdDate, int numberOfFlags = 0, bool isHidden = false)
        //{
        //    this.ReviewId = reviewId;
        //    this.UserId = userId;
        //    this.Rating = rating;
        //    this.Content = content;
        //    this.CreatedDate = createdDate;
        //    this.NumberOfFlags = numberOfFlags;
        //    this.IsHidden = isHidden;
        //}
         public Review()
        {
            this.Content = string.Empty;
            this.CreatedDate = DateTime.Now;
            this.IsActive = false;
            this.NumberOfFlags = 0;
            this.IsHidden = false;
            this.User = new User();
            this.Rating = new Rating();

        }

        public Review(int reviewId, Guid userId, Rating rating, string content, DateTime createdDate, int numberOfFlags = 0, bool isHidden = false)
        {
            this.ReviewId = reviewId;
            this.RatingId = rating.RatingId;
            this.UserId = userId;
            this.Rating = rating;
            this.Content = content;
            this.CreatedDate = createdDate;
            this.NumberOfFlags = numberOfFlags;
            this.IsHidden = isHidden;
            this.IsActive = false;
        }
        public int ReviewId { get; set; }

        public int RatingId { get; set; }

        public Guid UserId { get; set; }

        public string Content { get; set; }

        public DateTime? CreationDate { get; set; }

        public bool? IsActive { get; set; }

        public Rating Rating { get; set; }

        public User User { get; set; }

        public DateTime CreatedDate { get; set; }

        public int NumberOfFlags { get; set; }

        public bool IsHidden { get; set; }
    }
}
