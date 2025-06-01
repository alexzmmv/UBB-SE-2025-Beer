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
        private const int SUCCES_REVIEW_ID = 123;
        private const int FAILURE_REVIEW_ID = -1;

        public ReviewsServiceAddReviewTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.validReviewDto = new ReviewDTO
            {
                ReviewId = 0,
                Content = "Valid review",
                RatingValue = 5,
                CreatedDate = DateTime.UtcNow,
                IsHidden = false,
                DrinkId = 1,
                UserId = Guid.NewGuid()
            };

            this.invalidReviewDto = new ReviewDTO(); // empty or invalid

            // Setup mock for valid DTO
            this.mockReviewsRepository
                .Setup(repo => repo.AddReview(It.Is<ReviewDTO>(dto => ReviewValidator.IsValid(dto))))
                .ReturnsAsync(SUCCES_REVIEW_ID);

            // Setup mock for invalid DTO (should not be called, but in case)
            this.mockReviewsRepository
                .Setup(repo => repo.AddReview(It.Is<ReviewDTO>(dto => !ReviewValidator.IsValid(dto))))
                .ReturnsAsync(FAILURE_REVIEW_ID);
        }

        [Fact]
        public async Task AddReview_WithValidReview_ReturnsReviewId()
        {
            // Act
            int result = await this.reviewsService.AddReview(this.validReviewDto);

            // Assert
            Assert.Equal(SUCCES_REVIEW_ID, result);
        }

        [Fact]
        public async Task AddReview_WithInvalidReview_ReturnsFAILURE_REVIEW_ID()
        {
            // Act
            int result = await this.reviewsService.AddReview(this.invalidReviewDto);

            // Assert
            Assert.Equal(FAILURE_REVIEW_ID, result);
        }

        [Fact]
        public async Task AddReview_WhenRepositoryThrowsException_ReturnsFAILURE_REVIEW_ID()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repo => repo.AddReview(It.IsAny<ReviewDTO>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            int result = await this.reviewsService.AddReview(this.validReviewDto);

            // Assert
            Assert.Equal(FAILURE_REVIEW_ID, result);
        }
    }
}
