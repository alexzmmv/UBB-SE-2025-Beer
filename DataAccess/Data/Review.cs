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
            this.RatingValue = 0;
            this.CreatedDate = DateTime.Now;
            this.IsActive = false;
            this.NumberOfFlags = 0;
            this.IsHidden = false;
            this.User = new User();
            this.Drink = new Drink();

        }

        public Review(int reviewId, Guid userId, int drinkId, float ratingValue, string content, DateTime createdDate, int numberOfFlags = 0, bool isHidden = false)
        {
            this.ReviewId = reviewId;
            this.DrinkId = drinkId;
            this.UserId = userId;
            this.RatingValue = ratingValue;
            this.Content = content;
            this.CreatedDate = createdDate;
            this.NumberOfFlags = numberOfFlags;
            this.IsHidden = isHidden;
            this.IsActive = false;
        }
        public int ReviewId { get; set; }

        public int DrinkId { get; set; }

        public Guid UserId { get; set; }

        public string Content { get; set; }

        public bool? IsActive { get; set; }

        public User User { get; set; }
        
        public Drink Drink { get; set; }

        public DateTime CreatedDate { get; set; }

        public int NumberOfFlags { get; set; }

        public bool IsHidden { get; set; }
        
        public float? RatingValue { get; set; }
    }
}
