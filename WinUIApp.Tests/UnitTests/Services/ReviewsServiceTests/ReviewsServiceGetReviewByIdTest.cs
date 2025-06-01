using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetReviewByIdTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private const int EXISTING_REVIEW_ID = 1;
        private const int NON_EXISTING_REVIEW_ID = 999;

        private readonly ReviewDTO expectedReviewDto = new()
        {
            ReviewId = EXISTING_REVIEW_ID,
            Content = "Sample review",
            RatingValue = 5,
        };

        public ReviewsServiceGetReviewByIdTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewById(EXISTING_REVIEW_ID))
                .ReturnsAsync(this.expectedReviewDto);

            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewById(NON_EXISTING_REVIEW_ID))
                .ReturnsAsync((ReviewDTO?)null);

            this.mockReviewsRepository
                .Setup(repo => repo.GetReviewById(It.Is<int>(id => id < 0)))
                .ThrowsAsync(new Exception("Invalid review ID"));
        }

        [Fact]
        public async Task GetReviewById_WhenReviewExists_ReturnsReviewDto()
        {
            // Act
            ReviewDTO? actualReviewDto = await this.reviewsService.GetReviewById(EXISTING_REVIEW_ID);

            // Assert
            Assert.NotNull(actualReviewDto);
            Assert.Equal(this.expectedReviewDto.ReviewId, actualReviewDto!.ReviewId);
        }

        [Fact]
        public async Task GetReviewById_WhenReviewDoesNotExist_ReturnsNull()
        {
            // Act
            ReviewDTO? actualReviewDto = await this.reviewsService.GetReviewById(NON_EXISTING_REVIEW_ID);

            // Assert
            Assert.Null(actualReviewDto);
        }

        [Fact]
        public async Task GetReviewById_WhenRepositoryThrowsException_ReturnsNull()
        {
            // Arrange
            int invalidReviewId = -1;

            // Act
            ReviewDTO? actualReviewDto = await this.reviewsService.GetReviewById(invalidReviewId);

            // Assert
            Assert.Null(actualReviewDto);
        }
    }
}
