using Moq;
using DataAccess.DTOModels;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.IRepository;
using DataAccess.Service;

namespace WinUIApp.Tests.UnitTests.Services.ReviewsServiceTests
{
    public class ReviewsServiceGetReviewsWithUserInfoByDrinkTest
    {
        private readonly Mock<IReviewsRepository> mockReviewsRepository;
        private readonly ReviewsService reviewsService;

        private readonly int drinkId;
        private readonly List<ReviewWithUserDTO> expectedReviews;

        public ReviewsServiceGetReviewsWithUserInfoByDrinkTest()
        {
            this.mockReviewsRepository = new Mock<IReviewsRepository>();
            this.reviewsService = new ReviewsService(this.mockReviewsRepository.Object);

            this.drinkId = 42;

            this.expectedReviews = new List<ReviewWithUserDTO>
            {
                new ReviewWithUserDTO
                {
                    ReviewId = 1,
                    DrinkId = this.drinkId,
                    UserId = Guid.NewGuid(),
                    Content = "First review",
                    CreatedDate = DateTime.UtcNow,
                    RatingValue = 4,
                    NumberOfFlags = 0,
                    IsHidden = false,
                    Username = "user_one",
                    EmailAddress = "userone@example.com"
                },
                new ReviewWithUserDTO
                {
                    ReviewId = 2,
                    DrinkId = this.drinkId,
                    UserId = Guid.NewGuid(),
                    Content = "Second review",
                    CreatedDate = DateTime.UtcNow,
                    RatingValue = 5,
                    NumberOfFlags = 1,
                    IsHidden = true,
                    Username = "user_two",
                    EmailAddress = "usertwo@example.com"
                }
            };

            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsWithUserInfoByDrinkId(this.drinkId))
                .ReturnsAsync(this.expectedReviews);
        }

        [Fact]
        public async Task GetReviewsWithUserInfoByDrink_WhenRepositoryReturnsReviews_ReturnsExpectedReviews()
        {
            // Act
            List<ReviewWithUserDTO> actualReviews = await this.reviewsService.GetReviewsWithUserInfoByDrink(this.drinkId);

            // Assert
            Assert.Equal(this.expectedReviews, actualReviews);
        }

        [Fact]
        public async Task GetReviewsWithUserInfoByDrink_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            this.mockReviewsRepository
                .Setup(repository => repository.GetReviewsWithUserInfoByDrinkId(this.drinkId))
                .ThrowsAsync(new Exception("Simulated exception"));

            // Act
            List<ReviewWithUserDTO> actualReviews = await this.reviewsService.GetReviewsWithUserInfoByDrink(this.drinkId);

            // Assert
            Assert.Empty(actualReviews);
        }
    }
}
