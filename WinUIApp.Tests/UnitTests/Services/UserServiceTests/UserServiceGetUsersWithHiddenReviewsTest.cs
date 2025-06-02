using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceGetUsersWithHiddenReviewsTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceGetUsersWithHiddenReviewsTest()
        {
            this.userRepositoryMock = new Mock<IUserRepository>();
            this.userService = new UserService(this.userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUsersWithHiddenReviews_HasUsers_ReturnsUsersList()
        {
            // Arrange
            List<User> expectedUsers = new List<User>
            {
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "user1",
                    EmailAddress = "user1@example.com",
                    AssignedRole = RoleType.User
                },
                new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "user2",
                    EmailAddress = "user2@example.com",
                    AssignedRole = RoleType.User
                }
            };

            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersWithHiddenReviews())
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await this.userService.GetUsersWithHiddenReviews();

            // Assert
            Assert.Equal(expectedUsers.Count, result.Count);
            Assert.Equal(expectedUsers[0].UserId, result[0].UserId);
            Assert.Equal(expectedUsers[0].Username, result[0].Username);
            Assert.Equal(expectedUsers[1].UserId, result[1].UserId);
            Assert.Equal(expectedUsers[1].Username, result[1].Username);
        }

        [Fact]
        public async Task GetUsersWithHiddenReviews_NoUsers_ReturnsEmptyList()
        {
            // Arrange
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersWithHiddenReviews())
                .ReturnsAsync(new List<User>());

            // Act
            var result = await this.userService.GetUsersWithHiddenReviews();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersWithHiddenReviews_Exception_ReturnsEmptyList()
        {
            // Arrange
            this.userRepositoryMock.Setup(userRepository => userRepository.GetUsersWithHiddenReviews())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await this.userService.GetUsersWithHiddenReviews();

            // Assert
            Assert.Empty(result);
        }
    }
}