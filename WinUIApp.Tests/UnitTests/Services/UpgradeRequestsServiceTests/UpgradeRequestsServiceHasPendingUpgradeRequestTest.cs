using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UpgradeRequestsServiceTests
{
    public class UpgradeRequestsServiceHasPendingUpgradeRequestTest
    {
        private readonly Mock<IUpgradeRequestsRepository> mockUpgradeRequestsRepository;
        private readonly Mock<IRolesRepository> mockRolesRepository;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly UpgradeRequestsService upgradeRequestsService;

        private readonly Guid USER_WITH_PENDING_REQUEST = Guid.NewGuid();
        private readonly Guid USER_WITHOUT_PENDING_REQUEST = Guid.NewGuid();
        private readonly Guid USER_THAT_THROWS = Guid.NewGuid();

        public UpgradeRequestsServiceHasPendingUpgradeRequestTest()
        {
            this.mockUpgradeRequestsRepository = new Mock<IUpgradeRequestsRepository>();
            this.mockRolesRepository = new Mock<IRolesRepository>();
            this.mockUserRepository = new Mock<IUserRepository>();

            this.upgradeRequestsService = new UpgradeRequestsService(
                this.mockUpgradeRequestsRepository.Object,
                this.mockRolesRepository.Object,
                this.mockUserRepository.Object
            );

            // Setup mock behaviors
            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.HasPendingUpgradeRequest(USER_WITH_PENDING_REQUEST))
                .ReturnsAsync(true);

            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.HasPendingUpgradeRequest(USER_WITHOUT_PENDING_REQUEST))
                .ReturnsAsync(false);

            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.HasPendingUpgradeRequest(USER_THAT_THROWS))
                .ThrowsAsync(new Exception("Simulated error"));
        }

        [Fact]
        public async Task HasPendingUpgradeRequest_WhenUserHasPendingRequest_ReturnsTrue()
        {
            // Act
            var result = await this.upgradeRequestsService.HasPendingUpgradeRequest(USER_WITH_PENDING_REQUEST);

            // Assert
            Assert.True(result);
            this.mockUpgradeRequestsRepository.Verify(repo => repo.HasPendingUpgradeRequest(USER_WITH_PENDING_REQUEST), Times.Once);
        }

        [Fact]
        public async Task HasPendingUpgradeRequest_WhenUserHasNoPendingRequest_ReturnsFalse()
        {
            // Act
            var result = await this.upgradeRequestsService.HasPendingUpgradeRequest(USER_WITHOUT_PENDING_REQUEST);

            // Assert
            Assert.False(result);
            this.mockUpgradeRequestsRepository.Verify(repo => repo.HasPendingUpgradeRequest(USER_WITHOUT_PENDING_REQUEST), Times.Once);
        }

        [Fact]
        public async Task HasPendingUpgradeRequest_WhenRepositoryThrows_ReturnsFalse()
        {
            // Act
            var result = await this.upgradeRequestsService.HasPendingUpgradeRequest(USER_THAT_THROWS);

            // Assert
            Assert.False(result); // Should return false on exception
            this.mockUpgradeRequestsRepository.Verify(repo => repo.HasPendingUpgradeRequest(USER_THAT_THROWS), Times.Once);
        }
    }
}