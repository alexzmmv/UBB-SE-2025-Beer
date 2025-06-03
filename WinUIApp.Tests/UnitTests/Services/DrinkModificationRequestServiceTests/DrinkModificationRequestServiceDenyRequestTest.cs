using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.DrinkModificationRequestServiceTests
{
    public class DrinkModificationRequestServiceDenyRequestTest
    {
        private readonly Mock<IDrinkModificationRequestRepository> mockDrinkModificationRequestRepository;
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkModificationRequestService service;

        private readonly int existingRequestId = 1;
        private readonly int nonExistingRequestId = 999;
        private readonly Guid testUserId = Guid.NewGuid();

        private readonly DrinkModificationRequestDTO existingRequestWithNewDrink;
        private readonly DrinkModificationRequestDTO existingRequestWithoutNewDrink;

        public DrinkModificationRequestServiceDenyRequestTest()
        {
            mockDrinkModificationRequestRepository = new Mock<IDrinkModificationRequestRepository>();
            mockDrinkRepository = new Mock<IDrinkRepository>();

            existingRequestWithNewDrink = new DrinkModificationRequestDTO
            {
                DrinkModificationRequestId = existingRequestId,
                NewDrinkId = 42,
                OldDrinkId = null,
                ModificationType = DataAccess.Constants.DrinkModificationRequestType.Add,
                RequestingUserId = testUserId
            };

            existingRequestWithoutNewDrink = new DrinkModificationRequestDTO
            {
                DrinkModificationRequestId = existingRequestId,
                NewDrinkId = null,
                OldDrinkId = 10,
                ModificationType = DataAccess.Constants.DrinkModificationRequestType.Edit,
                RequestingUserId = testUserId
            };

            service = new DrinkModificationRequestService(
                mockDrinkModificationRequestRepository.Object,
                mockDrinkRepository.Object);
        }

        [Fact]
        public async Task DenyRequest_ExistingRequestWithNewDrink_DeletesRequestAndDeletesNewDrink()
        {
            // Arrange
            mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetModificationRequest(existingRequestId))
                .ReturnsAsync(existingRequestWithNewDrink);

            mockDrinkModificationRequestRepository
                .Setup(repo => repo.DeleteRequest(existingRequestId))
                .Returns(Task.CompletedTask);

            mockDrinkRepository
                .Setup(repo => repo.DeleteDrink(existingRequestWithNewDrink.NewDrinkId.Value));

            // Act
            await service.DenyRequest(existingRequestId, testUserId);

            // Assert
            mockDrinkModificationRequestRepository.Verify(repo => repo.GetModificationRequest(existingRequestId), Times.Once);
            mockDrinkModificationRequestRepository.Verify(repo => repo.DeleteRequest(existingRequestId), Times.Once);
            mockDrinkRepository.Verify(repo => repo.DeleteDrink(existingRequestWithNewDrink.NewDrinkId.Value), Times.Once);
        }

        [Fact]
        public async Task DenyRequest_ExistingRequestWithoutNewDrink_DeletesRequestButDoesNotDeleteDrink()
        {
            // Arrange
            mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetModificationRequest(existingRequestId))
                .ReturnsAsync(existingRequestWithoutNewDrink);

            mockDrinkModificationRequestRepository
                .Setup(repo => repo.DeleteRequest(existingRequestId))
                .Returns(Task.CompletedTask);

            // Act
            await service.DenyRequest(existingRequestId, testUserId);

            // Assert
            mockDrinkModificationRequestRepository.Verify(repo => repo.GetModificationRequest(existingRequestId), Times.Once);
            mockDrinkModificationRequestRepository.Verify(repo => repo.DeleteRequest(existingRequestId), Times.Once);
            mockDrinkRepository.Verify(repo => repo.DeleteDrink(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DenyRequest_NonExistingRequest_ThrowsInvalidOperationException()
        {
            // Arrange
            mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetModificationRequest(nonExistingRequestId))
                .ReturnsAsync((DrinkModificationRequestDTO?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.DenyRequest(nonExistingRequestId, testUserId));

            Assert.Equal($"Modification request {nonExistingRequestId} not found.", exception.Message);

            mockDrinkModificationRequestRepository.Verify(repo => repo.GetModificationRequest(nonExistingRequestId), Times.Once);
            mockDrinkModificationRequestRepository.Verify(repo => repo.DeleteRequest(It.IsAny<int>()), Times.Never);
            mockDrinkRepository.Verify(repo => repo.DeleteDrink(It.IsAny<int>()), Times.Never);
        }
    }
}
