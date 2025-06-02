using DataAccess.AutoChecker;
using DataAccess.Service;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.CheckersServiceTests
{
    public class CheckersServiceRunAutoCheckTest
    {
        private readonly Mock<IReviewService> reviewServiceMock;
        private readonly Mock<IAutoCheck> autoCheckMock;
        private readonly CheckersService checkersService;

        public CheckersServiceRunAutoCheckTest()
        {
            this.reviewServiceMock = new Mock<IReviewService>();
            this.autoCheckMock = new Mock<IAutoCheck>();
            this.checkersService = new CheckersService(this.reviewServiceMock.Object, this.autoCheckMock.Object);
        }

        [Fact]
        public async Task RunAutoCheck_NullReviews_ReturnsEmptyList()
        {
            // Act
            List<string> result = await this.checkersService.RunAutoCheck(null);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_EmptyReviews_ReturnsEmptyList()
        {
            // Arrange
            List<Review> reviews = new();

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_ReviewWithNullContent_SkipsReview()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = null }
            };

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
            this.autoCheckMock.Verify(x => x.AutoCheckReview(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RunAutoCheck_OffensiveReview_HidesReviewAndReturnsMessage()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "Offensive content" }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Offensive content")).ReturnsAsync(true);

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Single(result);
            Assert.Contains("Review 1 is offensive. Hiding the review.", result);
        }

        [Fact]
        public async Task RunAutoCheck_OffensiveReview_CallsHideReview()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "Offensive content" }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Offensive content")).ReturnsAsync(true);

            // Act
            await this.checkersService.RunAutoCheck(reviews);

            // Assert
            this.reviewServiceMock.Verify(x => x.HideReview(1), Times.Once);
        }

        [Fact]
        public async Task RunAutoCheck_OffensiveReview_CallsResetReviewFlags()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "Offensive content" }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Offensive content")).ReturnsAsync(true);

            // Act
            await this.checkersService.RunAutoCheck(reviews);

            // Assert
            this.reviewServiceMock.Verify(x => x.ResetReviewFlags(1), Times.Once);
        }

        [Fact]
        public async Task RunAutoCheck_NonOffensiveReview_ReturnsNotOffensiveMessage()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "Good content" }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Good content")).ReturnsAsync(false);

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Single(result);
            Assert.Contains("Review 1 is not offensive.", result);
        }

        [Fact]
        public async Task RunAutoCheck_NonOffensiveReview_DoesNotCallHideReview()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "Good content" }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Good content")).ReturnsAsync(false);

            // Act
            await this.checkersService.RunAutoCheck(reviews);

            // Assert
            this.reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task RunAutoCheck_ExceptionThrown_ReturnsEmptyList()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "Test content" }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Test content")).ThrowsAsync(new Exception("Test exception"));

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_MultipleReviews_ProcessesAllReviews()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "Good content" },
                new Review { ReviewId = 2, Content = "Bad content" }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Good content")).ReturnsAsync(false);
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Bad content")).ReturnsAsync(true);

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task RunAutoCheck_NullReviewInList_SkipsNullReview()
        {
            // Arrange
            List<Review> reviews = new()
            {
                null,
                new Review { ReviewId = 2, Content = "Valid content" }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Valid content")).ReturnsAsync(false);

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Single(result);
            Assert.Contains("Review 2 is not offensive.", result);
        }

        [Fact]
        public async Task RunAutoCheck_EmptyStringContent_ProcessesReview()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = string.Empty }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview(string.Empty)).ReturnsAsync(false);

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Single(result);
            Assert.Contains("Review 1 is not offensive.", result);
        }

        [Fact]
        public async Task RunAutoCheck_WhitespaceContent_ProcessesReview()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "   " }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("   ")).ReturnsAsync(false);

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Single(result);
            Assert.Contains("Review 1 is not offensive.", result);
        }

        [Fact]
        public async Task RunAutoCheck_HideReviewFails_ContinuesProcessing()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "Offensive content" }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Offensive content")).ReturnsAsync(true);
            this.reviewServiceMock.Setup(x => x.HideReview(1)).ThrowsAsync(new Exception("Hide failed"));

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_ResetReviewFlagsFails_ContinuesProcessing()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "Offensive content" }
            };
            this.autoCheckMock.Setup(x => x.AutoCheckReview("Offensive content")).ReturnsAsync(true);
            this.reviewServiceMock.Setup(x => x.ResetReviewFlags(1)).ThrowsAsync(new Exception("Reset failed"));

            // Act
            List<string> result = await this.checkersService.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task RunAutoCheck_NullAutoCheck_ThrowsException()
        {
            // Arrange
            List<Review> reviews = new()
            {
                new Review { ReviewId = 1, Content = "Test content" }
            };
            CheckersService checkersServiceWithNullAutoCheck = new(this.reviewServiceMock.Object, null);

            // Act
            List<string> result = await checkersServiceWithNullAutoCheck.RunAutoCheck(reviews);

            // Assert
            Assert.Empty(result);
        }
    }
}