using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Services;
using WinUIApp.WebAPI.Constants.ErrorMessages;
using WinUIApp.WebAPI.Extensions;
using WinUiApp.Data.Data;
using Xunit;
using System;
using DataAccess.IRepository;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class RatingServiceUpdateRatingTest
    {
        private readonly RatingService service;
        private readonly Mock<IRatingRepository> repository;

        public RatingServiceUpdateRatingTest()
        {
            repository = new Mock<IRatingRepository>();
            service = new RatingService(repository.Object);
        }

        [Fact]
        public void UpdateRating_WhenValidRating_ReturnsUpdatedModel()
        {
            // Arrange
            var dto = new RatingDTO
            {
                DrinkId = 1,
                UserId = 1,
                RatingValue = 4,
                RatingDate = DateTime.UtcNow,
                IsActive = true
            };

            var returnedDataModel = new Rating
            {
                RatingId = 10,
                DrinkId = dto.DrinkId,
                UserId = dto.UserId,
                RatingValue = dto.RatingValue,
                RatingDate = dto.RatingDate,
                IsActive = dto.IsActive
            };

            repository.Setup(repository => repository.UpdateRating(It.IsAny<Rating>()))
                       .Returns(returnedDataModel);

            // Act
            var result = service.UpdateRating(dto);

            // Assert
            Assert.Equal(dto.DrinkId, result.DrinkId);
        }

        [Fact]
        public void UpdateRating_WhenValidRating_CallsRepositoryOnce()
        {
            // Arrange
            var dto = new RatingDTO
            {
                DrinkId = 1,
                UserId = 1,
                RatingValue = 4,
                RatingDate = DateTime.UtcNow,
                IsActive = true
            };

            repository.Setup(repository => repository.UpdateRating(It.IsAny<Rating>()))
                       .Returns(dto.ToDataModel());

            // Act
            service.UpdateRating(dto);

            // Assert
            repository.Verify(repository => repository.UpdateRating(It.IsAny<Rating>()), Times.Once);
        }

        [Fact]
        public void UpdateRating_WhenInvalidRating_ThrowsArgumentException()
        {
            // Arrange
            var invalidDto = new RatingDTO { DrinkId = 1, UserId = 1, RatingValue = 8 };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => service.UpdateRating(invalidDto));
            Assert.Equal(ServiceErrorMessages.INVALID_RATING_VALUE, exception.Message);
        }
    }
}
