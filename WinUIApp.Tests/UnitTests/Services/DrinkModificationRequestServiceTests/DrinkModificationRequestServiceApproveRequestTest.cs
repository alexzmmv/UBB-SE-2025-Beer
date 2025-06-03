using DataAccess.DTOModels;
using DataAccess.IRepository;
using DataAccess.Service;
using Moq;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Models;

namespace WinUIApp.Tests.UnitTests.Services.DrinkModificationRequestServiceTests
{
    public class DrinkModificationRequestServiceApproveRequestTest
    {
        private readonly Mock<IDrinkModificationRequestRepository> mockDrinkModificationRequestRepository;
        private readonly Mock<IDrinkRepository> mockDrinkRepository;
        private readonly DrinkModificationRequestService service;

        private readonly int modificationRequestId = 1;
        private readonly Guid testUserId = Guid.NewGuid();

        public DrinkModificationRequestServiceApproveRequestTest()
        {
            mockDrinkModificationRequestRepository = new Mock<IDrinkModificationRequestRepository>();
            mockDrinkRepository = new Mock<IDrinkRepository>();

            service = new DrinkModificationRequestService(
                mockDrinkModificationRequestRepository.Object,
                mockDrinkRepository.Object);
        }

        [Fact]
        public async Task ApproveRequest_AddType_UpdatesNewDrinkAndDeletesRequest()
        {
            // Arrange
            var request = new DrinkModificationRequestDTO
            {
                DrinkModificationRequestId = modificationRequestId,
                ModificationType = DataAccess.Constants.DrinkModificationRequestType.Add,
                NewDrinkId = 10,
                RequestingUserId = testUserId
            };

            var newDrink = new DrinkDTO
            {
                DrinkId = 10,
                IsRequestingApproval = true
            };

            mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetModificationRequest(modificationRequestId))
                .ReturnsAsync(request);

            mockDrinkRepository
                .Setup(repo => repo.GetDrinkById(request.NewDrinkId.Value))
                .Returns(newDrink);

            mockDrinkRepository
                .Setup(repo => repo.UpdateDrink(It.IsAny<DrinkDTO>()));

            mockDrinkModificationRequestRepository
                .Setup(repo => repo.DeleteRequest(modificationRequestId))
                .Returns(Task.CompletedTask);

            // Act
            await service.ApproveRequest(modificationRequestId, testUserId);

            // Assert
            Assert.False(newDrink.IsRequestingApproval);
            mockDrinkRepository.Verify(repo => repo.UpdateDrink(newDrink), Times.Once);
            mockDrinkModificationRequestRepository.Verify(repo => repo.DeleteRequest(modificationRequestId), Times.Once);
        }

        [Fact]
        public async Task ApproveRequest_EditType_UpdatesOldDrinkDeletesNewDrinkAndDeletesRequest()
        {
            // Arrange
            var request = new DrinkModificationRequestDTO
            {
                DrinkModificationRequestId = modificationRequestId,
                ModificationType = DataAccess.Constants.DrinkModificationRequestType.Edit,
                OldDrinkId = 5,
                NewDrinkId = 10,
                RequestingUserId = testUserId
            };

            var oldDrink = new DrinkDTO
            {
                DrinkId = 5,
                DrinkName = "OldName",
                DrinkImageUrl = "oldurl",
                CategoryList = new List<Category> { new Category(1, "OldCategory") },
                DrinkBrand = new Brand { BrandId = 1, BrandName = "OldBrand" },
                AlcoholContent = 5.0f
            };

            var updatedDrink = new DrinkDTO
            {
                DrinkId = 10,
                DrinkName = "UpdatedName",
                DrinkImageUrl = "updatedurl",
                CategoryList = new List<Category> { new Category(2, "UpdatedCategory") },
                DrinkBrand = new Brand { BrandId = 2, BrandName = "UpdatedBrand" },
                AlcoholContent = 7.5f
            };

            mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetModificationRequest(modificationRequestId))
                .ReturnsAsync(request);

            mockDrinkRepository
                .Setup(repo => repo.GetDrinkById(request.OldDrinkId.Value))
                .Returns(oldDrink);

            mockDrinkRepository
                .Setup(repo => repo.GetDrinkById(request.NewDrinkId.Value))
                .Returns(updatedDrink);

            mockDrinkRepository
                .Setup(repo => repo.UpdateDrink(oldDrink));

            mockDrinkRepository
                .Setup(repo => repo.DeleteDrink(request.NewDrinkId.Value));

            mockDrinkModificationRequestRepository
                .Setup(repo => repo.DeleteRequest(modificationRequestId))
                .Returns(Task.CompletedTask);

            // Act
            await service.ApproveRequest(modificationRequestId, testUserId);

            // Assert
            Assert.Equal(updatedDrink.DrinkName, oldDrink.DrinkName);
            Assert.Equal(updatedDrink.DrinkImageUrl, oldDrink.DrinkImageUrl);
            Assert.Equal(updatedDrink.CategoryList, oldDrink.CategoryList);
            Assert.Equal(updatedDrink.DrinkBrand, oldDrink.DrinkBrand);
            Assert.Equal(updatedDrink.AlcoholContent, oldDrink.AlcoholContent);

            mockDrinkRepository.Verify(repo => repo.UpdateDrink(oldDrink), Times.Once);
            mockDrinkRepository.Verify(repo => repo.DeleteDrink(request.NewDrinkId.Value), Times.Once);
            mockDrinkModificationRequestRepository.Verify(repo => repo.DeleteRequest(modificationRequestId), Times.Once);
        }

        [Fact]
        public async Task ApproveRequest_RemoveType_DeletesOldDrinkAndDeletesRequest()
        {
            // Arrange
            var request = new DrinkModificationRequestDTO
            {
                DrinkModificationRequestId = modificationRequestId,
                ModificationType = DataAccess.Constants.DrinkModificationRequestType.Remove,
                OldDrinkId = 5,
                RequestingUserId = testUserId
            };

            mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetModificationRequest(modificationRequestId))
                .ReturnsAsync(request);

            mockDrinkRepository
                .Setup(repo => repo.DeleteDrink(request.OldDrinkId.Value));

            mockDrinkModificationRequestRepository
                .Setup(repo => repo.DeleteRequest(modificationRequestId))
                .Returns(Task.CompletedTask);

            // Act
            await service.ApproveRequest(modificationRequestId, testUserId);

            // Assert
            mockDrinkRepository.Verify(repo => repo.DeleteDrink(request.OldDrinkId.Value), Times.Once);
            mockDrinkModificationRequestRepository.Verify(repo => repo.DeleteRequest(modificationRequestId), Times.Once);
        }

        [Fact]
        public async Task ApproveRequest_UnknownModificationType_ThrowsInvalidOperationException()
        {
            // Arrange
            var request = new DrinkModificationRequestDTO
            {
                DrinkModificationRequestId = modificationRequestId,
                ModificationType = (DataAccess.Constants.DrinkModificationRequestType)999,
                RequestingUserId = testUserId
            };

            mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetModificationRequest(modificationRequestId))
                .ReturnsAsync(request);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.ApproveRequest(modificationRequestId, testUserId));

            Assert.Equal($"Unknown modification type: {request.ModificationType}", ex.Message);

            mockDrinkModificationRequestRepository.Verify(repo => repo.GetModificationRequest(modificationRequestId), Times.Once);
            mockDrinkModificationRequestRepository.Verify(repo => repo.DeleteRequest(It.IsAny<int>()), Times.Never);
            mockDrinkRepository.Verify(repo => repo.UpdateDrink(It.IsAny<DrinkDTO>()), Times.Never);
            mockDrinkRepository.Verify(repo => repo.DeleteDrink(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task ApproveRequest_RequestNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            mockDrinkModificationRequestRepository
                .Setup(repo => repo.GetModificationRequest(modificationRequestId))
                .ReturnsAsync((DrinkModificationRequestDTO?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.ApproveRequest(modificationRequestId, testUserId));

            Assert.Equal($"Modification request {modificationRequestId} not found.", ex.Message);

            mockDrinkModificationRequestRepository.Verify(repo => repo.GetModificationRequest(modificationRequestId), Times.Once);
            mockDrinkModificationRequestRepository.Verify(repo => repo.DeleteRequest(It.IsAny<int>()), Times.Never);
            mockDrinkRepository.Verify(repo => repo.UpdateDrink(It.IsAny<DrinkDTO>()), Times.Never);
            mockDrinkRepository.Verify(repo => repo.DeleteDrink(It.IsAny<int>()), Times.Never);
        }
    }
}
