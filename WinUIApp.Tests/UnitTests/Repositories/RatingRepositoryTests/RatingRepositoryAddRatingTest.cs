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
    public class RatingRepositoryAddRatingTest
    {
        private readonly Mock<DbSet<Rating>> mockRatingSet;
        private readonly Mock<IAppDbContext> mockDbContext;
        private readonly RatingRepository repository;
        private readonly List<Rating> ratings;

        public RatingRepositoryAddRatingTest()
        {
            ratings = new List<Rating>();

            mockRatingSet = new Mock<DbSet<Rating>>();
            mockRatingSet.Setup(set => set.Add(It.IsAny<Rating>()))
                          .Callback<Rating>(rating => ratings.Add(rating));

            mockDbContext = new Mock<IAppDbContext>();
            mockDbContext.Setup(database => database.Ratings).Returns(mockRatingSet.Object);
            mockDbContext.Setup(database => database.SaveChanges()).Returns(1);

            repository = new RatingRepository(mockDbContext.Object);
        }

        [Fact]
        public void AddRating_Adds_Rating_To_DbSet()
        {
            var rating = new Rating { DrinkId = 1, UserId = 1, RatingValue = 4.0f };

            repository.AddRating(rating);

            Assert.Single(ratings);
        }

        [Fact]
        public void AddRating_Sets_RatingDate_When_Null()
        {
            var rating = new Rating { DrinkId = 2, UserId = 2, RatingValue = 3.5f };

            var result = repository.AddRating(rating);

            Assert.NotNull(result.RatingDate);
        }

        [Fact]
        public void AddRating_Sets_IsActive_True_When_Null()
        {
            var rating = new Rating { DrinkId = 3, UserId = 3, RatingValue = 5.0f };

            var result = repository.AddRating(rating);

            Assert.True(result.IsActive.Value);
        }

        [Fact]
        public void AddRating_Calls_SaveChanges_Once()
        {
            var rating = new Rating { DrinkId = 4, UserId = 4, RatingValue = 2.0f };

            repository.AddRating(rating);

            mockDbContext.Verify(database => database.SaveChanges(), Times.Once);
        }
    }
}
