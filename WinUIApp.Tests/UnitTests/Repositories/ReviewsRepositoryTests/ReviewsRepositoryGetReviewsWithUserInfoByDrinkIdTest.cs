using DataAccess.DTOModels;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;

namespace WinUIApp.Tests.UnitTests.Repositories.ReviewsRepositoryTests
{
    public class ReviewsRepositoryGetReviewsWithUserInfoByDrinkIdTest
    {
        private readonly Mock<IAppDbContext> mockAppDbContext;
        private readonly ReviewsRepository reviewsRepository;
        private readonly List<Review> reviewData;
        private readonly List<User> userData;
        private readonly Mock<DbSet<Review>> mockReviewDbSet;

        public ReviewsRepositoryGetReviewsWithUserInfoByDrinkIdTest()
        {
            this.mockAppDbContext = new Mock<IAppDbContext>();

            Guid userIdOne = Guid.NewGuid();
            Guid userIdTwo = Guid.NewGuid();

            this.userData = new List<User>
            {
                new User { UserId = userIdOne, Username = "UserOne", EmailAddress = "userone@example.com" },
                new User { UserId = userIdTwo, Username = "UserTwo", EmailAddress = "usertwo@example.com" }
            };

            this.reviewData = new List<Review>
            {
                new Review
                {
                    ReviewId = 1,
                    DrinkId = 100,
                    UserId = userIdOne,
                    RatingValue = 4,
                    Content = "Tasty",
                    CreatedDate = DateTime.UtcNow,
                    NumberOfFlags = 1,
                    IsHidden = false,
                    User = this.userData[0]
                },
                new Review
                {
                    ReviewId = 2,
                    DrinkId = 200,
                    UserId = userIdTwo,
                    RatingValue = 2,
                    Content = "Bitter",
                    CreatedDate = DateTime.UtcNow,
                    NumberOfFlags = 3,
                    IsHidden = true,
                    User = this.userData[1]
                },
                new Review
                {
                    ReviewId = 3,
                    DrinkId = 100,
                    UserId = userIdTwo,
                    RatingValue = 5,
                    Content = "Excellent",
                    CreatedDate = DateTime.UtcNow,
                    NumberOfFlags = 0,
                    IsHidden = false,
                    User = this.userData[1]
                }
            };

            this.mockReviewDbSet = this.reviewData.AsQueryable().BuildMockDbSet();
            this.mockAppDbContext.Setup(context => context.Reviews).Returns(this.mockReviewDbSet.Object);

            this.reviewsRepository = new ReviewsRepository(this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task GetReviewsWithUserInfoByDrinkId_WhenReviewsExist_ReturnsCorrectUserInfo()
        {
            // Arrange
            int existingDrinkId = 100;

            // Act
            List<ReviewWithUserDTO> result = await this.reviewsRepository.GetReviewsWithUserInfoByDrinkId(existingDrinkId);

            // Assert
            string expectedUsername = "UserOne";
            Assert.Contains(result, dto => dto.Username == expectedUsername);
        }

        [Fact]
        public async Task GetReviewsWithUserInfoByDrinkId_WhenReviewsExist_ReturnsOnlyMatchingDrinkId()
        {
            // Arrange
            int targetDrinkId = 100;

            // Act
            List<ReviewWithUserDTO> result = await this.reviewsRepository.GetReviewsWithUserInfoByDrinkId(targetDrinkId);

            // Assert
            Assert.All(result, dto => Assert.Equal(targetDrinkId, dto.DrinkId));
        }

        [Fact]
        public async Task GetReviewsWithUserInfoByDrinkId_WhenNoReviewsExistForDrinkId_ReturnsEmptyList()
        {
            // Arrange
            int nonExistentDrinkId = 999;

            // Act
            List<ReviewWithUserDTO> result = await this.reviewsRepository.GetReviewsWithUserInfoByDrinkId(nonExistentDrinkId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetReviewsWithUserInfoByDrinkId_WhenUserIsNull_ReturnsEmptyUsernameAndEmail()
        {
            // Arrange
            int drinkIdWithMissingUser = 500;
            Guid orphanUserId = Guid.NewGuid();

            this.reviewData.Add(new Review
            {
                ReviewId = 4,
                DrinkId = drinkIdWithMissingUser,
                UserId = orphanUserId,
                RatingValue = 1,
                Content = "No user",
                CreatedDate = DateTime.UtcNow,
                NumberOfFlags = 2,
                IsHidden = true,
                User = null
            });

            var extendedMockSet = this.reviewData.AsQueryable().BuildMockDbSet();
            this.mockAppDbContext.Setup(context => context.Reviews).Returns(extendedMockSet.Object);

            // Act
            List<ReviewWithUserDTO> result = await this.reviewsRepository.GetReviewsWithUserInfoByDrinkId(drinkIdWithMissingUser);

            // Assert
            Assert.True(result.All(dto => dto.Username == string.Empty && dto.EmailAddress == string.Empty));
        }
    }
}