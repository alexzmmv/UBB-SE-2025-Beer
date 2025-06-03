using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.UpgradeRequestsServiceTests
{
    public class UpgradeRequestsServiceRemoveUpgradeRequestByIdentifierTest
    {
        private readonly Mock<IUpgradeRequestsRepository> mockUpgradeRequestsRepository;
        private readonly Mock<IRolesRepository> mockRolesRepository;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly UpgradeRequestsService service;

        private const int EXISTING_REQUEST_ID = 1;
        private const int NON_EXISTING_REQUEST_ID = 999;

        public UpgradeRequestsServiceRemoveUpgradeRequestByIdentifierTest()
        {
            this.mockUpgradeRequestsRepository = new Mock<IUpgradeRequestsRepository>();
            this.mockRolesRepository = new Mock<IRolesRepository>();
            this.mockUserRepository = new Mock<IUserRepository>();

            this.service = new UpgradeRequestsService(
                this.mockUpgradeRequestsRepository.Object,
                this.mockRolesRepository.Object,
                this.mockUserRepository.Object);

            // Setup repository to complete successfully on existing ID
            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.RemoveUpgradeRequestByIdentifier(EXISTING_REQUEST_ID))
                .Returns(Task.CompletedTask);

            // Setup repository to throw exception on non-existing ID
            this.mockUpgradeRequestsRepository
                .Setup(repo => repo.RemoveUpgradeRequestByIdentifier(NON_EXISTING_REQUEST_ID))
                .ThrowsAsync(new Exception("Upgrade request not found"));
        }

        [Fact]
        public async Task RemoveUpgradeRequestByIdentifier_WhenCalledWithExistingId_CallsRepositoryRemove()
        {
            // Act
            await this.service.RemoveUpgradeRequestByIdentifier(EXISTING_REQUEST_ID);

            // Assert
            this.mockUpgradeRequestsRepository.Verify(repo => repo.RemoveUpgradeRequestByIdentifier(EXISTING_REQUEST_ID), Times.Once);
        }

        [Fact]
        public async Task RemoveUpgradeRequestByIdentifier_WhenRepositoryThrowsException_DoesNotThrow()
        {
            // Act & Assert
            var exception = await Record.ExceptionAsync(() => this.service.RemoveUpgradeRequestByIdentifier(NON_EXISTING_REQUEST_ID));
            Assert.Null(exception);
        }
    }
}