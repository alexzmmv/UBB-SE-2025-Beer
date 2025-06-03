using DataAccess.IRepository;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UpgradeRequestsServiceTests
{
    public class UpgradeRequestsServiceRetrieveUpgradeRequestByIdentifierTest
    {
        private readonly Mock<IUpgradeRequestsRepository> mockUpgradeRequestsRepository;
        private readonly Mock<IRolesRepository> mockRolesRepository;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly UpgradeRequestsService service;

        private const int EXISTING_REQUEST_ID = 1;
        private const int NON_EXISTING_REQUEST_ID = 999;

        public UpgradeRequestsServiceRetrieveUpgradeRequestByIdentifierTest()
        {
            this.mockUpgradeRequestsRepository = new Mock<IUpgradeRequestsRepository>();
            this.mockRolesRepository = new Mock<IRolesRepository>();
            this.mockUserRepository = new Mock<IUserRepository>();

            this.service = new UpgradeRequestsService(
                this.mockUpgradeRequestsRepository.Object,
                this.mockRolesRepository.Object,
                this.mockUserRepository.Object);

            // Setup to return an UpgradeRequest for existing ID
            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.RetrieveUpgradeRequestByIdentifier(EXISTING_REQUEST_ID))
                .ReturnsAsync(new UpgradeRequest(EXISTING_REQUEST_ID, Guid.NewGuid(), "Test User"));

            // Setup to throw exception for non-existing ID
            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.RetrieveUpgradeRequestByIdentifier(NON_EXISTING_REQUEST_ID))
                .ThrowsAsync(new Exception("Upgrade request not found"));
        }

        [Fact]
        public async Task RetrieveUpgradeRequestByIdentifier_WhenCalledWithExistingId_ReturnsUpgradeRequest()
        {
            // Act
            UpgradeRequest? result = await this.service.RetrieveUpgradeRequestByIdentifier(EXISTING_REQUEST_ID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(EXISTING_REQUEST_ID, result.UpgradeRequestId);
            this.mockUpgradeRequestsRepository.Verify(repo => repo.RetrieveUpgradeRequestByIdentifier(EXISTING_REQUEST_ID), Times.Once);
        }

        [Fact]
        public async Task RetrieveUpgradeRequestByIdentifier_WhenRepositoryThrowsException_ReturnsNull()
        {
            // Act
            UpgradeRequest? result = await this.service.RetrieveUpgradeRequestByIdentifier(NON_EXISTING_REQUEST_ID);

            // Assert
            Assert.Null(result);
            this.mockUpgradeRequestsRepository.Verify(repo => repo.RetrieveUpgradeRequestByIdentifier(NON_EXISTING_REQUEST_ID), Times.Once);
        }
    }
}