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
            dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);
            dbContextMock.Setup(x => x.Reviews).Returns(reviewsDbSetMock.Object);
            userRepository = new UserRepository(dbContextMock.Object);
        }

        [Fact]
        public async Task GetUsersWithHiddenReviews_Success_ReturnsUsers()
        {
            // Arrange
            var user1 = new User { UserId = Guid.NewGuid(), Username = "user1" };
            var user2 = new User { UserId = Guid.NewGuid(), Username = "user2" };
            var review1 = new Review { ReviewId = 1, UserId = user1.UserId, IsHidden = true, User = user1 };
            var review2 = new Review { ReviewId = 2, UserId = user2.UserId, IsHidden = false, User = user2 };

            users.AddRange(new[] { user1, user2 });
            reviews.AddRange(new[] { review1, review2 });

            var localUsersDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            var localReviewsDbSetMock = AsyncQueryableHelper.CreateDbSetMock(reviews);
            dbContextMock.Setup(x => x.Users).Returns(localUsersDbSetMock.Object);
            dbContextMock.Setup(x => x.Reviews).Returns(localReviewsDbSetMock.Object);

            // Act
            var result = await userRepository.GetUsersWithHiddenReviews();

            // Assert
            Assert.Single(result);
            Assert.Equal(user1.UserId, result[0].UserId);
        }

        [Fact]
        public async Task GetUsersWithHiddenReviews_NoHiddenReviews_ReturnsEmptyList()
        {
            // Arrange
            var user = new User { UserId = Guid.NewGuid(), Username = "user" };
            var review = new Review { ReviewId = 1, UserId = user.UserId, IsHidden = false };

            users.Add(user);
            reviews.Add(review);

            var localUsersDbSetMock = AsyncQueryableHelper.CreateDbSetMock(users);
            var localReviewsDbSetMock = AsyncQueryableHelper.CreateDbSetMock(reviews);
            dbContextMock.Setup(x => x.Users).Returns(localUsersDbSetMock.Object);
            dbContextMock.Setup(x => x.Reviews).Returns(localReviewsDbSetMock.Object);

            // Act
            var result = await userRepository.GetUsersWithHiddenReviews();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersWithHiddenReviews_DbSetThrowsException_Throws()
        {
            // Arrange
            dbContextMock.Setup(x => x.Reviews).Throws(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userRepository.GetUsersWithHiddenReviews());
        }
    }
} 