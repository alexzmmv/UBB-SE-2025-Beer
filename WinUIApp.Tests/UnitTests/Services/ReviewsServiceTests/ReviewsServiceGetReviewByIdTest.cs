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

        private const int ExistingReviewId = 1;
        private const int NonExistingReviewId = 999;

        private readonly ReviewDTO ExpectedReviewDto = new ReviewDTO
        {
            ReviewId = ExistingReviewId,
            Content = "Sample review",
            RatingValue = 5,
        };

        public ReviewsServiceGetReviewByIdTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            mockReviewsRepository
                .Setup(repo => repo.GetReviewById(ExistingReviewId))
                .ReturnsAsync(ExpectedReviewDto);

            mockReviewsRepository
                .Setup(repo => repo.GetReviewById(NonExistingReviewId))
                .ReturnsAsync((ReviewDTO?)null);

            mockReviewsRepository
                .Setup(repo => repo.GetReviewById(It.Is<int>(id => id < 0)))
                .ThrowsAsync(new Exception("Invalid review ID"));
        }

        [Fact]
        public async Task GetReviewById_WhenReviewExists_ReturnsReviewDto()
        {
            // Act
            var actualReviewDto = await reviewsService.GetReviewById(ExistingReviewId);

            // Assert
            Assert.NotNull(actualReviewDto);
            Assert.Equal(ExpectedReviewDto.ReviewId, actualReviewDto!.ReviewId);
        }

        [Fact]
        public async Task GetReviewById_WhenReviewDoesNotExist_ReturnsNull()
        {
            // Act
            var actualReviewDto = await reviewsService.GetReviewById(NonExistingReviewId);

            // Assert
            Assert.Null(actualReviewDto);
        }

        [Fact]
        public async Task GetReviewById_WhenRepositoryThrowsException_ReturnsNull()
        {
            // Arrange
            int invalidReviewId = -1;

            // Act
            var actualReviewDto = await reviewsService.GetReviewById(invalidReviewId);

            // Assert
            Assert.Null(actualReviewDto);
        }
    }
}
