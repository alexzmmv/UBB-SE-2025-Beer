using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceChangeRoleToUserTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;
        private readonly Role userRole;

        public UserServiceChangeRoleToUserTest()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            userService = new UserService(userRepositoryMock.Object);
            userRole = new Role(RoleType.User, "User");
        }

        [Fact]
        public async Task ChangeRoleToUser_Success()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            // Act
            await this.userService.ChangeRoleToUser(userId, this.userRole);

            // Assert
            this.userRepositoryMock.Verify(userRepository => userRepository.ChangeRoleToUser(userId, this.userRole), Times.Once);
        }

        [Fact]
        public async Task ChangeRoleToUser_RepositoryThrowsException_HandlesGracefully()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            this.userRepositoryMock.Setup(userRepository => userRepository.ChangeRoleToUser(userId, this.userRole))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await this.userService.ChangeRoleToUser(userId, this.userRole);
            this.userRepositoryMock.Verify(userRepository => userRepository.ChangeRoleToUser(userId, this.userRole), Times.Once);
        }
    }
} 