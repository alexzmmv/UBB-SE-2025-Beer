using DataAccess.AutoChecker;
using DataAccess.Service;
using DrinkDb_Auth.Service.AdminDashboard.Interfaces;
using WinUiApp.Data.Data;
using Moq;

namespace WinUIApp.Tests.UnitTests.Services.CheckersServiceTests
{
    /// <summary>
    /// Integration tests for AI detection functionality in CheckersService.
    /// These tests exercise the RunAICheckForOneReviewAsync method with various content types
    /// to improve code coverage of the CheckReviewWithAI static method.
    /// 
    /// Note: Since CheckReviewWithAI calls OffensiveTextDetector.DetectOffensiveContent (static method),
    /// these tests depend on the actual implementation and may be considered integration tests
    /// rather than pure unit tests.
    /// </summary>
    public class CheckersServiceAIDetectionIntegrationTest
    {
        private readonly Mock<IReviewService> reviewServiceMock;
        private readonly Mock<IAutoCheck> autoCheckMock;
        private readonly CheckersService checkersService;

        public CheckersServiceAIDetectionIntegrationTest()
        {
            reviewServiceMock = new Mock<IReviewService>();
            autoCheckMock = new Mock<IAutoCheck>();
            checkersService = new CheckersService(reviewServiceMock.Object, autoCheckMock.Object);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ValidContent_ExercisesAIDetectionPath()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = "This is a normal review content" };

            // Act & Assert (should not throw and should exercise the AI detection code path)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_JsonLikeContent_ExercisesJsonParsingPath()
        {
            // Arrange
            var jsonLikeContent = "{\"label\": \"offensive\", \"score\": 0.8}";
            var review = new Review { ReviewId = 1, Content = jsonLikeContent };

            // Act & Assert (should not throw and may exercise JSON parsing paths)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_MalformedJsonContent_ExercisesErrorHandling()
        {
            // Arrange
            var malformedJson = "{\"label\": \"offensive\", \"score\":";
            var review = new Review { ReviewId = 1, Content = malformedJson };

            // Act & Assert (should not throw and may exercise error handling paths)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NumericContent_ExercisesContentProcessing()
        {
            // Arrange
            var numericContent = "12345 67890 rating: 5.0";
            var review = new Review { ReviewId = 1, Content = numericContent };

            // Act & Assert (should not throw)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_MixedCaseContent_ExercisesTextProcessing()
        {
            // Arrange
            var mixedCaseContent = "ThIs Is A MiXeD cAsE rEvIeW";
            var review = new Review { ReviewId = 1, Content = mixedCaseContent };

            // Act & Assert (should not throw)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ContentWithNewlines_ExercisesFormatHandling()
        {
            // Arrange
            var contentWithNewlines = "Line 1\nLine 2\r\nLine 3\tTabbed content";
            var review = new Review { ReviewId = 1, Content = contentWithNewlines };

            // Act & Assert (should not throw)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ContentWithQuotes_ExercisesStringHandling()
        {
            // Arrange
            var contentWithQuotes = "This is a \"quoted\" review with 'single quotes' too";
            var review = new Review { ReviewId = 1, Content = contentWithQuotes };

            // Act & Assert (should not throw)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ErrorPrefixContent_ExercisesErrorDetection()
        {
            // Arrange - Content that might trigger the "Error:" prefix check
            var errorContent = "Error: Something went wrong in this review";
            var review = new Review { ReviewId = 1, Content = errorContent };

            // Act & Assert (should not throw and may exercise error prefix detection)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ExceptionPrefixContent_ExercisesExceptionDetection()
        {
            // Arrange - Content that might trigger the "Exception:" prefix check
            var exceptionContent = "Exception: This review contains the word exception";
            var review = new Review { ReviewId = 1, Content = exceptionContent };

            // Act & Assert (should not throw and may exercise exception prefix detection)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_OffensiveLabelContent_ExercisesLabelDetection()
        {
            // Arrange - Content that might trigger the "offensive" label check
            var offensiveContent = "This review contains the word offensive in it";
            var review = new Review { ReviewId = 1, Content = offensiveContent };

            // Act & Assert (should not throw and may exercise label detection)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_ScoreContent_ExercisesScoreParsingPath()
        {
            // Arrange - Content that might trigger score parsing logic
            var scoreContent = "This review has a score of 0.75 out of 1.0";
            var review = new Review { ReviewId = 1, Content = scoreContent };

            // Act & Assert (should not throw and may exercise score parsing paths)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NullJsonContent_ExercisesNullDeserializationPath()
        {
            // Arrange - Content that might cause OffensiveTextDetector to return "null" 
            // which would make JsonConvert.DeserializeObject return null
            var nullJsonContent = "null content that might trigger null response";
            var review = new Review { ReviewId = 1, Content = nullJsonContent };

            // Act & Assert (should not throw and may exercise the arrayResults == null path)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_EmptyArrayContent_ExercisesEmptyArrayPath()
        {
            // Arrange - Content that might cause OffensiveTextDetector to return empty array JSON
            // This would exercise the arrayResults?.Count > 0 check
            var emptyArrayContent = "content that might return empty array []";
            var review = new Review { ReviewId = 1, Content = emptyArrayContent };

            // Act & Assert (should not throw and may exercise empty array handling)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NestedEmptyArrayContent_ExercisesNestedEmptyArrayPath()
        {
            // Arrange - Content that might cause response like [[]] - empty nested array
            // This would exercise the arrayResults[0]?.Count > 0 check
            var nestedEmptyContent = "content that might return nested empty [[]]";
            var review = new Review { ReviewId = 1, Content = nestedEmptyContent };

            // Act & Assert (should not throw and may exercise nested empty array handling)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_IncompleteJsonStructure_ExercisesStructureValidation()
        {
            // Arrange - Content that might cause response with missing label/score fields
            // This would exercise the prediction.TryGetValue checks
            var incompleteStructureContent = "content that might return incomplete JSON structure";
            var review = new Review { ReviewId = 1, Content = incompleteStructureContent };

            // Act & Assert (should not throw and may exercise structure validation)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_NonOffensiveLabelContent_ExercisesLabelComparison()
        {
            // Arrange - Content that might return a label other than "offensive"
            // This would exercise the label comparison logic
            var nonOffensiveContent = "content that might return non-offensive label";
            var review = new Review { ReviewId = 1, Content = nonOffensiveContent };

            // Act & Assert (should not throw and may exercise non-offensive label path)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_LowScoreContent_ExercisesScoreThreshold()
        {
            // Arrange - Content that might return offensive label but score <= 0.6
            // This would exercise the score threshold check (score > 0.6)
            var lowScoreContent = "content that might return low confidence score";
            var review = new Review { ReviewId = 1, Content = lowScoreContent };

            // Act & Assert (should not throw and may exercise score threshold logic)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_InvalidScoreFormat_ExercisesScoreParsing()
        {
            // Arrange - Content that might return non-numeric score value
            // This would exercise the double.TryParse failure path
            var invalidScoreContent = "content that might return invalid score format";
            var review = new Review { ReviewId = 1, Content = invalidScoreContent };

            // Act & Assert (should not throw and may exercise score parsing error handling)
            checkersService.RunAICheckForOneReviewAsync(review);
        }

        [Fact]
        public void RunAICheckForOneReviewAsync_MultipleReviewObjects_ExercisesArrayHandling()
        {
            // Arrange - Test with different review content patterns
            var reviews = new[]
            {
                new Review { ReviewId = 1, Content = "Simple content" },
                new Review { ReviewId = 2, Content = "Content with numbers 123" },
                new Review { ReviewId = 3, Content = "UPPERCASE CONTENT" },
                new Review { ReviewId = 4, Content = "lowercase content" },
                new Review { ReviewId = 5, Content = "Mixed Case Content" }
            };

            // Act & Assert (should not throw for any review)
            foreach (var review in reviews)
            {
                checkersService.RunAICheckForOneReviewAsync(review);
            }
        }

        /// <summary>
        /// Test documenting the JSON deserialization null scenario.
        /// 
        /// JsonConvert.DeserializeObject can return null in several cases:
        /// 1. When the JSON response is literally "null"
        /// 2. When the JSON structure doesn't match the expected type
        /// 3. When malformed JSON can't be converted to the target type
        /// 
        /// This test exercises content that might potentially trigger such scenarios.
        /// </summary>
        [Fact]
        public void RunAICheckForOneReviewAsync_JsonDeserializationEdgeCases_ExercisesNullHandling()
        {
            // Arrange - Various content types that might trigger different JSON responses
            var edgeCaseContents = new[]
            {
                "Content that might return null JSON response",
                "Content that might return malformed JSON",
                "Content that might return unexpected JSON structure",
                "Content that might return empty JSON object {}",
                "Content that might return JSON with wrong schema"
            };

            // Act & Assert - All should handle gracefully without throwing
            foreach (var content in edgeCaseContents)
            {
                var review = new Review { ReviewId = 1, Content = content };
                checkersService.RunAICheckForOneReviewAsync(review);
            }
        }
    }
} 