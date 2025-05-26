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
    public class RatingRepositoryDeleteRatingTest
    {
        private readonly Mock<DbSet<Rating>> mockRatingSet;
        private readonly Mock<IAppDbContext> mockDbContext;
        private readonly RatingRepository repository;
        private readonly List<Rating> ratings;

        public RatingRepositoryDeleteRatingTest()
        {
            ratings = new List<Rating>
            {
                new Rating { RatingId = 1, DrinkId = 10, UserId = 20 },
                new Rating { RatingId = 2, DrinkId = 11, UserId = 21 }
            };

            var queryableRatings = ratings.AsQueryable();

            mockRatingSet = new Mock<DbSet<Rating>>();
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.Provider).Returns(queryableRatings.Provider);
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.Expression).Returns(queryableRatings.Expression);
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.ElementType).Returns(queryableRatings.ElementType);
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.GetEnumerator()).Returns(() => queryableRatings.GetEnumerator());

            mockRatingSet.Setup(set => set.Remove(It.IsAny<Rating>()))
                         .Callback<Rating>(rating => ratings.Remove(rating));

            mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(database => database.Ratings).Returns(mockRatingSet.Object);
            mockDbContext.Setup(database => database.SaveChanges()).Returns(1);

            repository = new RatingRepository(mockDbContext.Object);
        }

        [Fact]
        public void DeleteRating_Removes_Rating_From_DbSet()
        {
            repository.DeleteRating(1);

            Assert.DoesNotContain(ratings, rating => rating.RatingId == 1);
        }

        [Fact]
        public void DeleteRating_Calls_SaveChanges_Once()
        {
            repository.DeleteRating(2);

            mockDbContext.Verify(database => database.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteRating_When_Rating_Not_Found_Throws_Exception()
        {
            var invalidRatingId = 999;

            var exception = Assert.Throws<Exception>(() => repository.DeleteRating(invalidRatingId));

            Assert.Equal(RepositoryErrorMessages.EntityNotFound, exception.Message);
        }
    }
}
