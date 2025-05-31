using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;
using DataAccess.Extensions;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceAddReviewTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly ReviewDTO validReviewDto;
        private readonly ReviewDTO invalidReviewDto;
        private const int SuccessReviewId = 123;
        private const int FailureReviewId = -1;

        public ReviewsServiceAddReviewTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            validReviewDto = new ReviewDTO
            {
                ReviewId = 0,
                Content = "Valid review",
                RatingValue = 5,
                CreatedDate = DateTime.UtcNow,
                IsHidden = false,
                DrinkId = 1,
                UserId = Guid.NewGuid()
            };

            invalidReviewDto = new ReviewDTO(); // empty or invalid

            // Setup mock for valid DTO
            mockReviewsRepository
                .Setup(repo => repo.AddReview(It.Is<ReviewDTO>(dto => ReviewValidator.IsValid(dto))))
                .ReturnsAsync(SuccessReviewId);

            // Setup mock for invalid DTO (should not be called, but in case)
            mockReviewsRepository
                .Setup(repo => repo.AddReview(It.Is<ReviewDTO>(dto => !ReviewValidator.IsValid(dto))))
                .ReturnsAsync(FailureReviewId);
        }

        [Fact]
        public async Task AddReview_WithValidReview_ReturnsReviewId()
        {
            // Act
            int result = await reviewsService.AddReview(validReviewDto);

            // Assert
            Assert.Equal(SuccessReviewId, result);
        }

        [Fact]
        public async Task AddReview_WithInvalidReview_ReturnsFailureReviewId()
        {
            // Act
            int result = await reviewsService.AddReview(invalidReviewDto);

            // Assert
            Assert.Equal(FailureReviewId, result);
        }

        [Fact]
        public async Task AddReview_WhenRepositoryThrowsException_ReturnsFailureReviewId()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repo => repo.AddReview(It.IsAny<ReviewDTO>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            int result = await reviewsService.AddReview(validReviewDto);

            // Assert
            Assert.Equal(FailureReviewId, result);
        }
    }
}
