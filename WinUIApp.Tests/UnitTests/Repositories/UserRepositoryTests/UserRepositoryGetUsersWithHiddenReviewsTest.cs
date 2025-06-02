using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.Tests.UnitTests.TestHelpers;
using Moq;

namespace WinUIApp.Tests.UnitTests.Repositories.UserRepositoryTests
{
    public class UserRepositoryGetUsersWithHiddenReviewsTest
    {
        private readonly Mock<IAppDbContext> dbContextMock;
        private readonly Mock<DbSet<User>> usersDbSetMock;
        private readonly Mock<DbSet<Review>> reviewsDbSetMock;
        private readonly UserRepository userRepository;
        private readonly List<User> users;
        private readonly List<Review> reviews;

        public UserRepositoryGetUsersWithHiddenReviewsTest()
        {
            users = new List<User>();
            reviews = new List<Review>();
            usersDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            reviewsDbSetMock = AsyncQueryableHelper.CreateDbSetMock(reviews);
            dbContextMock = new Mock<IAppDbContext>();
            dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(usersDbSetMock.Object);
            dbContextMock.Setup(databaseContext => databaseContext.Reviews).Returns(reviewsDbSetMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task GetUsersWithHiddenReviews_Success_ReturnsUsers()
        {
            // Arrange
            User user1 = new User { UserId = Guid.NewGuid(), Username = "user1" };
            User user2 = new User { UserId = Guid.NewGuid(), Username = "user2" };
            Review review1 = new Review { ReviewId = 1, UserId = user1.UserId, IsHidden = true, User = user1 };
            Review review2 = new Review { ReviewId = 2, UserId = user2.UserId, IsHidden = false, User = user2 };

            this.users.AddRange(new[] { user1, user2 });
            this.reviews.AddRange(new[] { review1, review2 });

            Mock<DbSet<User>> localUsersDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            Mock<DbSet<Review>> localReviewsDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.reviews);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localUsersDbSetMock.Object);
            this.dbContextMock.Setup(databaseContext => databaseContext.Reviews).Returns(localReviewsDbSetMock.Object);

            // Act
            List<User> result = await this.userRepository.GetUsersWithHiddenReviews();

            // Assert
            Assert.Single(result);
            Assert.Equal(user1.UserId, result[0].UserId);
        }

        [Fact]
        public async Task GetUsersWithHiddenReviews_NoHiddenReviews_ReturnsEmptyList()
        {
            // Arrange
            User user = new User { UserId = Guid.NewGuid(), Username = "user" };
            Review review = new Review { ReviewId = 1, UserId = user.UserId, IsHidden = false };

            this.users.Add(user);
            this.reviews.Add(review);

            Mock<DbSet<User>> localUsersDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.users);
            Mock<DbSet<Review>> localReviewsDbSetMock = AsyncQueryableHelper.CreateDbSetMock(this.reviews);
            this.dbContextMock.Setup(databaseContext => databaseContext.Users).Returns(localUsersDbSetMock.Object);
            this.dbContextMock.Setup(databaseContext => databaseContext.Reviews).Returns(localReviewsDbSetMock.Object);

            // Act
            List<User> result = await this.userRepository.GetUsersWithHiddenReviews();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersWithHiddenReviews_DbSetThrowsException_Throws()
        {
            // Arrange
            dbContextMock.Setup(databaseContext => databaseContext.Reviews).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => this.userRepository.GetUsersWithHiddenReviews());
        }
    }
} 