using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceUpdateReviewVisibilityTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int VALID_REVIEW_ID = 1;
        private const bool NEW_VISIBILITY = true;

        public ReviewsServiceUpdateReviewVisibilityTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);
        }

        [Fact]
        public async Task UpdateReviewVisibility_WhenReviewIsValid_CallsUpdateReviewVisibility()
        {
            // Arrange
            int validRatingValue = 1;
            var validReviewDto = new ReviewDTO { ReviewId = VALID_REVIEW_ID, RatingValue = validRatingValue, Content = "Valid" };

            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewById(VALID_REVIEW_ID))
                .ReturnsAsync(validReviewDto);

            this.mockReviewsRepository
                .Setup(repo => repo.UpdateReviewVisibility(VALID_REVIEW_ID, NEW_VISIBILITY))
                .Returns(Task.CompletedTask);

            // Act
            await this.reviewsService.UpdateReviewVisibility(VALID_REVIEW_ID, NEW_VISIBILITY);

            // Assert
            this.mockReviewsRepository.Verify(
                repo => repo.UpdateReviewVisibility(VALID_REVIEW_ID, NEW_VISIBILITY),
                Times.Once);
        }

        [Fact]
        public async Task UpdateReviewVisibility_WhenReviewIsNull_DoesNotCallUpdateReviewVisibility()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewById(VALID_REVIEW_ID))
                .ReturnsAsync((ReviewDTO?)null);

            // Act
            await this.reviewsService.UpdateReviewVisibility(VALID_REVIEW_ID, NEW_VISIBILITY);

            // Assert
            this.mockReviewsRepository.Verify(
                repo => repo.UpdateReviewVisibility(It.IsAny<int>(), It.IsAny<bool>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateReviewVisibility_WhenReviewHasInvalidRating_DoesNotCallUpdateReviewVisibility()
        {
            // Arrange
            var invalidRatingValue = 6; // greater than 5 means invalid rating
            var invalidReviewDto = new ReviewDTO
            {
                ReviewId = VALID_REVIEW_ID,
                RatingValue = invalidRatingValue
            };

            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewById(VALID_REVIEW_ID))
                .ReturnsAsync(invalidReviewDto);

            // Act
            await this.reviewsService.UpdateReviewVisibility(VALID_REVIEW_ID, NEW_VISIBILITY);

            // Assert
            this.mockReviewsRepository.Verify(
                repo => repo.UpdateReviewVisibility(It.IsAny<int>(), It.IsAny<bool>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateReviewVisibility_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Arrange
            var validReviewDto = new ReviewDTO { ReviewId = VALID_REVIEW_ID };

            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewById(VALID_REVIEW_ID))
                .ReturnsAsync(validReviewDto);

            this.mockReviewsRepository
                .Setup(repo => repo.UpdateReviewVisibility(VALID_REVIEW_ID, NEW_VISIBILITY))
                .ThrowsAsync(new Exception("Repository exception"));

            // Act & Assert
            await this.reviewsService.UpdateReviewVisibility(VALID_REVIEW_ID, NEW_VISIBILITY);
        }
    }
}
