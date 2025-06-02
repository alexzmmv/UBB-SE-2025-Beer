using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.ServiceProxy;
using Moq;
using Moq.Protected;

namespace WinUIApp.Tests.UnitTests.ServiceProxies.OffensiveWords
{
    public class OffensiveWordsServiceProxyDeleteOffensiveWordAsyncTest
    {
        private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
        private readonly HttpClient httpClient;
        private readonly OffensiveWordsServiceProxy offensiveWordsServiceProxy;
        private readonly string baseUrl = "https://test-api.com";

        public OffensiveWordsServiceProxyDeleteOffensiveWordAsyncTest()
        {
            this.httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            this.httpClient = new HttpClient(this.httpMessageHandlerMock.Object);
            this.offensiveWordsServiceProxy = new OffensiveWordsServiceProxy(this.baseUrl);

            // Use reflection to set the private httpClient field
            var httpClientField = typeof(OffensiveWordsServiceProxy).GetField("httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            httpClientField?.SetValue(this.offensiveWordsServiceProxy, this.httpClient);
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_ValidWord_DeletesWordSuccessfully()
        {
            // Arrange
            string wordToDelete = "badword";
            string expectedUri = $"{this.baseUrl}/api/offensiveWords/delete/{Uri.EscapeDataString(wordToDelete)}";

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete &&
                        req.RequestUri.ToString() == expectedUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            await this.offensiveWordsServiceProxy.DeleteOffensiveWordAsync(wordToDelete);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete &&
                    req.RequestUri.ToString() == expectedUri),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_WordWithSpecialCharacters_EscapesUriCorrectly()
        {
            // Arrange
            string wordToDelete = "bad@word#test";
            string expectedUri = $"{this.baseUrl}/api/offensiveWords/delete/{Uri.EscapeDataString(wordToDelete)}";

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete &&
                        req.RequestUri.ToString() == expectedUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            await this.offensiveWordsServiceProxy.DeleteOffensiveWordAsync(wordToDelete);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete &&
                    req.RequestUri.ToString() == expectedUri),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_NullWord_ReturnsEarly()
        {
            // Arrange
            string nullWord = null;

            // Act
            await this.offensiveWordsServiceProxy.DeleteOffensiveWordAsync(nullWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_EmptyWord_ReturnsEarly()
        {
            // Arrange
            string emptyWord = string.Empty;

            // Act
            await this.offensiveWordsServiceProxy.DeleteOffensiveWordAsync(emptyWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_WhitespaceWord_ReturnsEarly()
        {
            // Arrange
            string whitespaceWord = "   ";

            // Act
            await this.offensiveWordsServiceProxy.DeleteOffensiveWordAsync(whitespaceWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_HttpRequestFails_ThrowsException()
        {
            // Arrange
            string wordToDelete = "badword";
            string expectedUri = $"{this.baseUrl}/api/offensiveWords/delete/{Uri.EscapeDataString(wordToDelete)}";

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete &&
                        req.RequestUri.ToString() == expectedUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                () => this.offensiveWordsServiceProxy.DeleteOffensiveWordAsync(wordToDelete));
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_HttpRequestThrowsException_ThrowsException()
        {
            // Arrange
            string wordToDelete = "badword";
            string expectedUri = $"{this.baseUrl}/api/offensiveWords/delete/{Uri.EscapeDataString(wordToDelete)}";

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete &&
                        req.RequestUri.ToString() == expectedUri),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                () => this.offensiveWordsServiceProxy.DeleteOffensiveWordAsync(wordToDelete));
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_WordNotFound_ThrowsException()
        {
            // Arrange
            string wordToDelete = "nonexistentword";
            string expectedUri = $"{this.baseUrl}/api/offensiveWords/delete/{Uri.EscapeDataString(wordToDelete)}";

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete &&
                        req.RequestUri.ToString() == expectedUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Word not found")
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                () => this.offensiveWordsServiceProxy.DeleteOffensiveWordAsync(wordToDelete));
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_Unauthorized_ThrowsException()
        {
            // Arrange
            string wordToDelete = "badword";
            string expectedUri = $"{this.baseUrl}/api/offensiveWords/delete/{Uri.EscapeDataString(wordToDelete)}";

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete &&
                        req.RequestUri.ToString() == expectedUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent("Unauthorized")
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                () => this.offensiveWordsServiceProxy.DeleteOffensiveWordAsync(wordToDelete));
        }

        [Fact]
        public async Task DeleteOffensiveWordAsync_LongWord_HandlesCorrectly()
        {
            // Arrange
            string longWord = new string('a', 500); // Very long word
            string expectedUri = $"{this.baseUrl}/api/offensiveWords/delete/{Uri.EscapeDataString(longWord)}";

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete &&
                        req.RequestUri.ToString() == expectedUri),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                });

            // Act
            await this.offensiveWordsServiceProxy.DeleteOffensiveWordAsync(longWord);

            // Assert
            this.httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete &&
                    req.RequestUri.ToString() == expectedUri),
                ItExpr.IsAny<CancellationToken>());
        }
    }
} 