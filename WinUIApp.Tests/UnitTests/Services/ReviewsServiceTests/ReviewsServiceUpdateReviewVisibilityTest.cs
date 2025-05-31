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

        private const int ValidReviewId = 1;
        private const bool NewVisibility = true;

        public ReviewsServiceUpdateReviewVisibilityTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);
        }

        [Fact]
        public async Task UpdateReviewVisibility_WhenReviewIsValid_CallsUpdateReviewVisibility()
        {
            // Arrange
            int validRatingValue = 1;
            var validReviewDto = new ReviewDTO { ReviewId = ValidReviewId, RatingValue = validRatingValue, Content = "Valid"};

            mockReviewsRepository
                .Setup(repo => repo.GetReviewById(ValidReviewId))
                .ReturnsAsync(validReviewDto);

            mockReviewsRepository
                .Setup(repo => repo.UpdateReviewVisibility(ValidReviewId, NewVisibility))
                .Returns(Task.CompletedTask);

            // Act
            await reviewsService.UpdateReviewVisibility(ValidReviewId, NewVisibility);

            // Assert
            mockReviewsRepository.Verify(
                repo => repo.UpdateReviewVisibility(ValidReviewId, NewVisibility),
                Times.Once);
        }

        [Fact]
        public async Task UpdateReviewVisibility_WhenReviewIsNull_DoesNotCallUpdateReviewVisibility()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repo => repo.GetReviewById(ValidReviewId))
                .ReturnsAsync((ReviewDTO?)null);

            // Act
            await reviewsService.UpdateReviewVisibility(ValidReviewId, NewVisibility);

            // Assert
            mockReviewsRepository.Verify(
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
                ReviewId = ValidReviewId,
                RatingValue = invalidRatingValue
            };

            mockReviewsRepository
                .Setup(repo => repo.GetReviewById(ValidReviewId))
                .ReturnsAsync(invalidReviewDto);

            // Act
            await reviewsService.UpdateReviewVisibility(ValidReviewId, NewVisibility);

            // Assert
            mockReviewsRepository.Verify(
                repo => repo.UpdateReviewVisibility(It.IsAny<int>(), It.IsAny<bool>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateReviewVisibility_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Arrange
            var validReviewDto = new ReviewDTO { ReviewId = ValidReviewId };

            mockReviewsRepository
                .Setup(repo => repo.GetReviewById(ValidReviewId))
                .ReturnsAsync(validReviewDto);

            mockReviewsRepository
                .Setup(repo => repo.UpdateReviewVisibility(ValidReviewId, NewVisibility))
                .ThrowsAsync(new Exception("Repository exception"));

            // Act & Assert
            await reviewsService.UpdateReviewVisibility(ValidReviewId, NewVisibility);
        }
    }
}
