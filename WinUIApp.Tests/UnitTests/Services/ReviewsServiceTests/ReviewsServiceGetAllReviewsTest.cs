using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetAllReviewsTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly List<ReviewDTO> allReviews;

        public ReviewsServiceGetAllReviewsTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.allReviews = new List<ReviewDTO>
            {
                new ReviewDTO { ReviewId = 1, Content = "Review 1" },
                new ReviewDTO { ReviewId = 2, Content = "Review 2" }
            };

            this.mockReviewsRepository
                .Setup(repository => repository.GetAllReviews())
                .ReturnsAsync(this.allReviews);
        }

        [Fact]
        public async Task GetAllReviews_WhenCalled_ReturnsAllReviews()
        {
            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetAllReviews();

            // Assert
            Assert.Equal(this.allReviews, actualReviews);
        }

        [Fact]
        public async Task GetAllReviews_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetAllReviews())
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            List<ReviewDTO> actualReviews = await this.reviewsService.GetAllReviews();

            // Assert
            Assert.Empty(actualReviews);
        }
    }
}
