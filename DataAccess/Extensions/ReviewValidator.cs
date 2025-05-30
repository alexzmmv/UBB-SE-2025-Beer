namespace DataAccess.Extensions
{
    using DataAccess.DTOModels;
    using WinUIApp.WebAPI.Constants;

    public static class ReviewValidator
    {
        public static bool IsValid(ReviewDTO review)
        {
            return IsContentValid(review.Content) && IsRatingValid(review.RatingValue);
        }

        private static bool IsContentValid(string content)
        {
            return !string.IsNullOrWhiteSpace(content) && content.Length <= ReviewDomainConstants.MaxContentLength;
        }

        private static bool IsRatingValid(float? rating)
        {
            if (!rating.HasValue)
                return false;

            return rating.Value >= ReviewDomainConstants.MinRatingValue && rating.Value <= ReviewDomainConstants.MaxRatingValue;
        }
    }
} 