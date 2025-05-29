using System;
using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;
using WinUIApp.WebAPI.Constants.ErrorMessages;
using WinUiApp.Data.Data;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class ReviewServiceAddReviewTest
    {
        private readonly ReviewService service;
        private readonly Mock<IReviewRepository> repository;

        public ReviewServiceAddReviewTest()
        {
            repository = new Mock<IReviewRepository>();
            service = new ReviewService(repository.Object);
        }

        [Fact]
        public void AddReview_WhenValid_SetsIsActiveTrue()
        {
            // Arrange
            var dto = new ReviewDTO
            {
                RatingId = 1,
                UserId = 1,
                Content = "Awesome product!"
            };

            Review? savedReview = null;

            repository.Setup(repository => repository.AddReview(It.IsAny<Review>()))
                       .Callback<Review>(review => savedReview = review)
                       .Returns<Review>(review => review);

            // Act
            var result = service.AddReview(dto);

            // Assert
            Assert.True(savedReview?.IsActive);
        }

        [Fact]
        public void AddReview_WhenContentTooLong_ThrowsArgumentException()
        {
            // Arrange
            var dto = new ReviewDTO
            {
                RatingId = 1,
                UserId = 1,
                Content = new string('a', 600) // invalid
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => service.AddReview(dto));
            Assert.Equal(ServiceErrorMessages.InvalidReview, exception.Message);
        }

        [Fact]
        public void AddReview_WhenContentIsWhitespace_ThrowsArgumentException()
        {
            // Arrange
            var dto = new ReviewDTO
            {
                RatingId = 1,
                UserId = 1,
                Content = "   " // invalid
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => service.AddReview(dto));
            Assert.Equal(ServiceErrorMessages.InvalidReview, exception.Message);
        }
    }
}
