using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Constants.ErrorMessages;
using WinUIApp.WebAPI.Repositories;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Repositories
{
    public class RatingRepositoryUpdateRatingTest
    {
        private readonly List<Rating> ratingList;
        private readonly Mock<DbSet<Rating>> mockRatingSet;
        private readonly Mock<IAppDbContext> mockDbContext;
        private readonly RatingRepository repository;

        public RatingRepositoryUpdateRatingTest()
        {
            ratingList = new List<Rating>
            {
                new Rating
                {
                    RatingId = 1,
                    DrinkId = 1,
                    UserId = 1,
                    RatingValue = 4.0f,
                    RatingDate = DateTime.UtcNow.AddDays(-1),
                    IsActive = true
                }
            };

            IQueryable<Rating> queryableRatings = ratingList.AsQueryable();

            mockRatingSet = new Mock<DbSet<Rating>>();
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.Provider).Returns(queryableRatings.Provider);
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.Expression).Returns(queryableRatings.Expression);
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.ElementType).Returns(queryableRatings.ElementType);
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.GetEnumerator()).Returns(() => queryableRatings.GetEnumerator());

            mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(context => context.Ratings).Returns(mockRatingSet.Object);
            mockDbContext.Setup(context => context.SaveChanges()).Returns(1);

            repository = new RatingRepository(mockDbContext.Object);
        }

        [Fact]
        public void UpdateRating_WhenRatingExists_UpdatesFields()
        {
            Rating updatedRating = new Rating
            {
                RatingId = 1,
                DrinkId = 2,
                UserId = 2,
                RatingValue = 3.5f,
                RatingDate = DateTime.UtcNow,
                IsActive = false
            };

            Rating result = repository.UpdateRating(updatedRating);

            Assert.Equal(2, result.DrinkId);
            Assert.Equal(2, result.UserId);
            Assert.Equal(3.5f, result.RatingValue);
            Assert.Equal(updatedRating.RatingDate, result.RatingDate);
            Assert.False(result.IsActive.Value);
        }

        [Fact]
        public void UpdateRating_WhenRatingDateIsNull_KeepsOldDate()
        {
            DateTime originalDate = ratingList[0].RatingDate.Value;

            Rating updatedRating = new Rating
            {
                RatingId = 1,
                DrinkId = 3,
                UserId = 3,
                RatingValue = 4.2f,
                RatingDate = null,
                IsActive = true
            };

            Rating result = repository.UpdateRating(updatedRating);

            Assert.Equal(originalDate, result.RatingDate);
        }

        [Fact]
        public void UpdateRating_WhenIsActiveIsNull_KeepsOldValue()
        {
            bool originalIsActive = ratingList[0].IsActive.Value;

            Rating updatedRating = new Rating
            {
                RatingId = 1,
                DrinkId = 4,
                UserId = 4,
                RatingValue = 4.9f,
                RatingDate = DateTime.UtcNow,
                IsActive = null
            };

            Rating result = repository.UpdateRating(updatedRating);

            Assert.Equal(originalIsActive, result.IsActive);
        }

        [Fact]
        public void UpdateRating_WhenRatingNotFound_ThrowsException()
        {
            Rating updatedRating = new Rating
            {
                RatingId = 999,
                DrinkId = 1,
                UserId = 1,
                RatingValue = 1.0f
            };

            Exception exception = Assert.Throws<Exception>(() => repository.UpdateRating(updatedRating));
            Assert.Equal(RepositoryErrorMessages.EntityNotFound, exception.Message);
        }

        [Fact]
        public void UpdateRating_CallsSaveChangesOnce()
        {
            Rating updatedRating = new Rating
            {
                RatingId = 1,
                DrinkId = 5,
                UserId = 5,
                RatingValue = 2.0f,
                RatingDate = DateTime.UtcNow,
                IsActive = true
            };

            repository.UpdateRating(updatedRating);

            mockDbContext.Verify(context => context.SaveChanges(), Times.Once);
        }
    }
}
