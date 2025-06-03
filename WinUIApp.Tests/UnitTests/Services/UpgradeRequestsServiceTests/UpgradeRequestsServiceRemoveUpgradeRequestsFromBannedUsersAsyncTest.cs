using DataAccess.Constants;
using DataAccess.IRepository;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UpgradeRequestsServiceTests
{
    public class UpgradeRequestsServiceRemoveUpgradeRequestsFromBannedUsersTest
    {
        private readonly Mock<IUpgradeRequestsRepository> mockUpgradeRequestsRepository;
        private readonly Mock<IRolesRepository> mockRolesRepository;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly UpgradeRequestsService service;

        public UpgradeRequestsServiceRemoveUpgradeRequestsFromBannedUsersTest()
        {
            this.mockUpgradeRequestsRepository = new Mock<IUpgradeRequestsRepository>();
            this.mockRolesRepository = new Mock<IRolesRepository>();
            this.mockUserRepository = new Mock<IUserRepository>();

            this.service = new UpgradeRequestsService(
                mockUpgradeRequestsRepository.Object,
                mockRolesRepository.Object,
                mockUserRepository.Object
            );
        }

        [Fact]
        public async Task RemoveUpgradeRequestsFromBannedUsersAsync_RemovesOnlyBannedUsersRequests()
        {
            // Arrange
            var bannedUserId = Guid.NewGuid();
            var normalUserId = Guid.NewGuid();

            var requests = new List<UpgradeRequest>
            {
                new UpgradeRequest(1, bannedUserId, "BannedUser"),
                new UpgradeRequest(2, normalUserId, "NormalUser")
            };

            mockUpgradeRequestsRepository
                .Setup(repo => repo.RetrieveAllUpgradeRequests())
                .ReturnsAsync(requests);

            mockUserRepository
                .Setup(repo => repo.GetRoleTypeForUser(bannedUserId))
                .ReturnsAsync(RoleType.Banned);

            mockUserRepository
                .Setup(repo => repo.GetRoleTypeForUser(normalUserId))
                .ReturnsAsync(RoleType.User);

            mockUpgradeRequestsRepository
                .Setup(repo => repo.RemoveUpgradeRequestByIdentifier(1))
                .Returns(Task.CompletedTask);

            // Act
            await service.RemoveUpgradeRequestsFromBannedUsersAsync();

            // Assert
            mockUpgradeRequestsRepository.Verify(repo => repo.RemoveUpgradeRequestByIdentifier(1), Times.Once);
            mockUpgradeRequestsRepository.Verify(repo => repo.RemoveUpgradeRequestByIdentifier(2), Times.Never);

            mockUserRepository.Verify(repo => repo.GetRoleTypeForUser(It.IsAny<Guid>()), Times.Exactly(2));
            mockUpgradeRequestsRepository.Verify(repo => repo.RetrieveAllUpgradeRequests(), Times.Once);
        }

        [Fact]
        public async Task RemoveUpgradeRequestsFromBannedUsersAsync_WhenExceptionOccurs_DoesNotThrow()
        {
            // Arrange
            mockUpgradeRequestsRepository
                .Setup(repo => repo.RetrieveAllUpgradeRequests())
                .ThrowsAsync(new Exception("Simulated error"));

            // Act
            var exception = await Record.ExceptionAsync(() => service.RemoveUpgradeRequestsFromBannedUsersAsync());

            // Assert
            Assert.Null(exception);
        }
    }
}