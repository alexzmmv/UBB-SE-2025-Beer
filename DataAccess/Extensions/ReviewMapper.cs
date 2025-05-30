namespace DataAccess.Extensions
{
    using WinUiApp.Data.Data;
    using DataAccess.DTOModels;

    public static class ReviewMapper
    {
        public static ReviewDTO ToDTO(Review review)
        {
            return new ReviewDTO
            {
                ReviewId = review.ReviewId,
                DrinkId = review.DrinkId,
                UserId = review.UserId,
                Content = review.Content,
                RatingValue = review.RatingValue,
                CreatedDate = review.CreatedDate,
                NumberOfFlags = review.NumberOfFlags,
                IsHidden = review.IsHidden
            };
        }

        public static Review ToEntity(ReviewDTO reviewDto)
        {
            return new Review
            {
                ReviewId = reviewDto.ReviewId,
                DrinkId = reviewDto.DrinkId,
                UserId = reviewDto.UserId,
                Content = reviewDto.Content,
                RatingValue = reviewDto.RatingValue,
                CreatedDate = reviewDto.CreatedDate,
                NumberOfFlags = reviewDto.NumberOfFlags,
                IsHidden = reviewDto.IsHidden
            };
        }
    }
} 