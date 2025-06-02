using DataAccess.AutoChecker;
using DataAccess.Service;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.CheckersServiceTests
{
    /// <summary>
    /// Tests for RunAICheckForOneReviewAsync method.
    /// Note: The CheckReviewWithAI method calls a static OffensiveTextDetector.DetectOffensiveContent method,
    /// which makes it difficult to test the AI detection logic paths in unit tests.
    /// These tests focus on the controllable behavior around the static method call.
    /// </summary>
    public class CheckersServiceRunAICheckForOneReviewAsyncTest
    {
        private readonly Mock<IReviewService> reviewServiceMock;
        private readonly Mock<IAutoCheck> autoCheckMock;
        private readonly CheckersService checkersService;

        public CheckersServiceRunAICheckForOneReviewAsyncTest()
        {
            reviewServiceMock = new Mock<IReviewService>();
            autoCheckMock = new Mock<IAutoCheck>();
            checkersService = new CheckersService(reviewServiceMock.Object, autoCheckMock.Object);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NullReview_DoesNotCallReviewService()
        {
            // Act
            checkersService.RunAICheckForOneReviewAsync(null);

            // Assert
            reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ReviewWithNullContent_DoesNotCallReviewService()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = null };

            // Act
            checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ReviewWithNullContent_DoesNotCallResetReviewFlags()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = null };

            // Act
            checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            reviewServiceMock.Verify(x => x.ResetReviewFlags(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ValidReviewNotOffensive_DoesNotCallHideReview()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = "Good content" };

            // Act
            checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ValidReviewNotOffensive_DoesNotCallResetReviewFlags()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = "Good content" };

            // Act
            checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            reviewServiceMock.Verify(x => x.ResetReviewFlags(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_EmptyContent_DoesNotCallReviewService()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = string.Empty };

            // Act
            checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_WhitespaceContent_DoesNotCallReviewService()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = "   " };

            // Act
            checkersService.RunAICheckForOneReviewAsync(review);

            // Assert
            reviewServiceMock.Verify(x => x.HideReview(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ExceptionThrown_DoesNotThrow()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = "Test content" };

            // Act & Assert (should not throw)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ExceptionInReviewService_DoesNotThrow()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = "Test content" };
            reviewServiceMock.Setup(x => x.HideReview(It.IsAny<int>())).Throws(new Exception("Service error"));

            // Act & Assert (should not throw)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NullReviewService_DoesNotThrow()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = "Test content" };
            var checkersServiceWithNullReviewService = new CheckersService(null, autoCheckMock.Object);

            // Act & Assert (should not throw)
            checkersServiceWithNullReviewService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_LongContent_DoesNotThrow()
        {
            // Arrange
            var longContent = new string('a', 10000); // Very long content
            var review = new Review { ReviewId = 1, Content = longContent };

            // Act & Assert (should not throw)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_SpecialCharactersContent_DoesNotThrow()
        {
            // Arrange
            var specialContent = "!@#$%^&*()_+{}|:\"<>?[]\\;',./ test content";
            var review = new Review { ReviewId = 1, Content = specialContent };

            // Act & Assert (should not throw)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_UnicodeContent_DoesNotThrow()
        {
            // Arrange
            var unicodeContent = "Test content with unicode: 🚀 こんにちは العربية";
            var review = new Review { ReviewId = 1, Content = unicodeContent };

            // Act & Assert (should not throw)
            checkersService.RunAICheckForOneReviewAsync(review);
        }
    }
} 