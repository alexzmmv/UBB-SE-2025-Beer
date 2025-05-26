using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Repositories;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Repositories
{
    public class RatingRepositoryGetAllTest
    {
        private readonly List<Rating> ratingList;
        private readonly Mock<DbSet<Rating>> mockRatingSet;
        private readonly Mock<IAppDbContext> mockDbContext;
        private readonly RatingRepository repository;

        public RatingRepositoryGetAllTest()
        {
            ratingList = new List<Rating>
            {
                new Rating { RatingId = 1, DrinkId = 10, UserId = 100, RatingValue = 4.0f },
                new Rating { RatingId = 2, DrinkId = 20, UserId = 101, RatingValue = 3.0f },
                new Rating { RatingId = 3, DrinkId = 10, UserId = 102, RatingValue = 5.0f }
            };

            IQueryable<Rating> queryableRatings = ratingList.AsQueryable();

            mockRatingSet = new Mock<DbSet<Rating>>();
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.Provider).Returns(queryableRatings.Provider);
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.Expression).Returns(queryableRatings.Expression);
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.ElementType).Returns(queryableRatings.ElementType);
            mockRatingSet.As<IQueryable<Rating>>().Setup(set => set.GetEnumerator()).Returns(() => queryableRatings.GetEnumerator());

            mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(context => context.Ratings).Returns(mockRatingSet.Object);

            repository = new RatingRepository(mockDbContext.Object);
        }

        [Fact]
        public void GetRatingById_ReturnsRating_WhenExists()
        {
            Rating? result = repository.GetRatingById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.RatingId);
        }

        [Fact]
        public void GetRatingById_ReturnsNull_WhenNotFound()
        {
            Rating? result = repository.GetRatingById(999);

            Assert.Null(result);
        }

        [Fact]
        public void GetAllRatings_ReturnsAllRatings()
        {
            List<Rating> result = repository.GetAllRatings();

            Assert.Equal(ratingList.Count, result.Count);
        }

        [Fact]
        public void GetRatingsByDrinkId_ReturnsCorrectRatings()
        {
            List<Rating> result = repository.GetRatingsByDrinkId(10);

            Assert.Equal(2, result.Count);
            Assert.All(result, rating => Assert.Equal(10, rating.DrinkId));
        }

        [Fact]
        public void GetRatingsByDrinkId_ReturnsEmptyList_WhenNoMatch()
        {
            List<Rating> result = repository.GetRatingsByDrinkId(999);

            Assert.Empty(result);
        }
    }
}

