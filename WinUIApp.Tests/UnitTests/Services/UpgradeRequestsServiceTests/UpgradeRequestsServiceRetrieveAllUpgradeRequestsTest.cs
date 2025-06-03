using DataAccess.IRepository;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UpgradeRequestsServiceTests
{
    public class UpgradeRequestsServiceRetrieveAllUpgradeRequestsTest
    {
        private readonly Mock<IUpgradeRequestsRepository> mockUpgradeRequestsRepository;
        private readonly Mock<IRolesRepository> mockRolesRepository;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly UpgradeRequestsService upgradeRequestsService;

        public UpgradeRequestsServiceRetrieveAllUpgradeRequestsTest()
        {
            this.mockUpgradeRequestsRepository = new Mock<IUpgradeRequestsRepository>();
            this.mockRolesRepository = new Mock<IRolesRepository>();
            this.mockUserRepository = new Mock<IUserRepository>();

            this.upgradeRequestsService = new UpgradeRequestsService(
                this.mockUpgradeRequestsRepository.Object,
                this.mockRolesRepository.Object,
                this.mockUserRepository.Object
            );
        }

        [Fact]
        public async Task RetrieveAllUpgradeRequests_WhenRepositoryReturnsData_ReturnsUpgradeRequests()
        {
            // Arrange
            var expectedRequests = new List<UpgradeRequest>
            {
                new UpgradeRequest(1, Guid.NewGuid(), "User1"),
                new UpgradeRequest(2, Guid.NewGuid(), "User2")
            };

            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.RetrieveAllUpgradeRequests())
                .ReturnsAsync(expectedRequests);

            // Act
            var result = await this.upgradeRequestsService.RetrieveAllUpgradeRequests();

            // Assert
            Assert.Equal(expectedRequests.Count, result.Count);
            Assert.Equal(expectedRequests, result);

            this.mockUpgradeRequestsRepository.Verify(repo => repo.RetrieveAllUpgradeRequests(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task RetrieveAllUpgradeRequests_WhenRepositoryThrowsException_ReturnsEmptyList()
        {
            // Arrange
            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.RetrieveAllUpgradeRequests())
                .ThrowsAsync(new Exception("Simulated failure"));

            // Act
            var result = await this.upgradeRequestsService.RetrieveAllUpgradeRequests();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            this.mockUpgradeRequestsRepository.Verify(repo => repo.RetrieveAllUpgradeRequests(), Times.AtLeastOnce);
        }
    }
}