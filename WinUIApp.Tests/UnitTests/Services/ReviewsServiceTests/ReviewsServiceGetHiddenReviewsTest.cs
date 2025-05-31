using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetHiddenReviewsTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly List<ReviewDTO> allReviews;
        private readonly List<ReviewDTO> expectedHiddenReviews;

        public ReviewsServiceGetHiddenReviewsTest()
        {
            mockReviewsRepository = new Mock<IReviewsRepository>();
            reviewsService = new ReviewsService(mockReviewsRepository.Object);

            allReviews = new List<ReviewDTO>
            {
                new ReviewDTO { ReviewId = 1, IsHidden = true },
                new ReviewDTO { ReviewId = 2, IsHidden = false },
                new ReviewDTO { ReviewId = 3, IsHidden = true }
            };

            expectedHiddenReviews = allReviews.Where(review => review.IsHidden).ToList();

            mockReviewsRepository
                .Setup(repository => repository.GetAllReviews())
                .ReturnsAsync(allReviews);
        }

        [Fact]
        public async Task GetHiddenReviews_WhenCalled_ReturnsOnlyHiddenReviews()
        {
            // Act
            List<ReviewDTO> actualHiddenReviews = await reviewsService.GetHiddenReviews();

            // Assert
            Assert.Equal(expectedHiddenReviews, actualHiddenReviews);
        }

        [Fact]
        public async Task GetHiddenReviews_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            mockReviewsRepository
                .Setup(repository => repository.GetAllReviews())
                .ThrowsAsync(new Exception("Repository failure"));

            // Act
            List<ReviewDTO> actualHiddenReviews = await reviewsService.GetHiddenReviews();

            // Assert
            Assert.Empty(actualHiddenReviews);
        }
    }
}
