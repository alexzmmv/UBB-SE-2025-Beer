using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Service;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UserServiceTests
{
    public class UserServiceUpdateUserAppealedTest
    {
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly UserService userService;

        public UserServiceUpdateUserAppealedTest()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            userService = new UserService(userRepositoryMock.Object);
        }

        [Fact]
        public async Task UpdateUserAppealed_Success()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                UserId = userId,
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.Banned,
                HasSubmittedAppeal = false
            };

            userRepositoryMock.Setup(userRepository => userRepository.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            await userService.UpdateUserAppleaed(user, true);

            // Assert
            userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.Is<User>(userEntity => 
                userEntity.UserId == userId && 
                userEntity.HasSubmittedAppeal)), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateUserAppealed_UserNotFound_NoUpdate()
        {
            // Arrange
            User? user = null;

            // Act
            await userService.UpdateUserAppleaed(user, true);

            // Assert
            userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUserAppealed_UpdateFails_NoException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                UserId = userId,
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.Banned,
                HasSubmittedAppeal = false
            };

            userRepositoryMock.Setup(userRepository => userRepository.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act
            await userService.UpdateUserAppleaed(user, true);

            // Assert
            userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.Is<User>(userEntity => 
                userEntity.UserId == userId && 
                userEntity.HasSubmittedAppeal)), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateUserAppealed_Exception_NoException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                UserId = userId,
                Username = "user",
                EmailAddress = "user@example.com",
                AssignedRole = RoleType.Banned,
                HasSubmittedAppeal = false
            };

            userRepositoryMock.Setup(userRepository => userRepository.UpdateUser(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await userService.UpdateUserAppleaed(user, true);

            // Assert
            userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.Is<User>(userEntity => 
                userEntity.UserId == userId && 
                userEntity.HasSubmittedAppeal)), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateUserAppealed_GetUserException_NoException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            User? user = null;

            // Act
            await userService.UpdateUserAppleaed(user, true);

            // Assert
            userRepositoryMock.Verify(userRepository => userRepository.UpdateUser(It.IsAny<User>()), Times.Never);
        }
    }
} 