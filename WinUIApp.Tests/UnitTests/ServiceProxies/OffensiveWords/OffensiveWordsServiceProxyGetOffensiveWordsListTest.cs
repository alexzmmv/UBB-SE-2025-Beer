using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Model.AdminDashboard;
using DataAccess.ServiceProxy;
using Moq;
using Moq.Protected;

namespace WinUIApp.Tests.UnitTests.ServiceProxies.OffensiveWords
{
    public class OffensiveWordsServiceProxyGetOffensiveWordsListTest
    {
        private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
        private readonly HttpClient httpClient;
        private readonly OffensiveWordsServiceProxy offensiveWordsServiceProxy;
        private readonly string baseUrl = "https://test-api.com";

        public OffensiveWordsServiceProxyGetOffensiveWordsListTest()
        {
            this.httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            this.httpClient = new HttpClient(this.httpMessageHandlerMock.Object);
            this.offensiveWordsServiceProxy = new OffensiveWordsServiceProxy(this.baseUrl);

            // Use reflection to set the private httpClient field
            var httpClientField = typeof(OffensiveWordsServiceProxy).GetField("httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            httpClientField?.SetValue(this.offensiveWordsServiceProxy, this.httpClient);
        }

        [Fact]
        public async Task GetOffensiveWordsList_ValidResponse_ReturnsHashSetOfWords()
        {
            // Arrange
            var offensiveWords = new List<OffensiveWord>
            {
                new OffensiveWord("badword1"),
                new OffensiveWord("badword2"),
                new OffensiveWord("offensive")
            };

            var jsonResponse = JsonSerializer.Serialize(offensiveWords);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await this.offensiveWordsServiceProxy.GetOffensiveWordsList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains("badword1", result);
            Assert.Contains("badword2", result);
            Assert.Contains("offensive", result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_EmptyResponse_ReturnsEmptyHashSet()
        {
            // Arrange
            var offensiveWords = new List<OffensiveWord>();
            var jsonResponse = JsonSerializer.Serialize(offensiveWords);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await this.offensiveWordsServiceProxy.GetOffensiveWordsList();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_CaseInsensitiveComparison_ReturnsCorrectHashSet()
        {
            // Arrange
            var offensiveWords = new List<OffensiveWord>
            {
                new OffensiveWord("BadWord"),
                new OffensiveWord("OFFENSIVE"),
                new OffensiveWord("inappropriate")
            };

            var jsonResponse = JsonSerializer.Serialize(offensiveWords);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await this.offensiveWordsServiceProxy.GetOffensiveWordsList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains("badword", result); // Should find regardless of case
            Assert.Contains("BADWORD", result); // Should find regardless of case
            Assert.Contains("offensive", result); // Should find regardless of case
            Assert.Contains("OFFENSIVE", result); // Should find regardless of case
        }

        [Fact]
        public async Task GetOffensiveWordsList_HttpRequestFails_ThrowsException()
        {
            // Arrange
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                () => this.offensiveWordsServiceProxy.GetOffensiveWordsList());
        }

        [Fact]
        public async Task GetOffensiveWordsList_HttpRequestThrowsException_ThrowsException()
        {
            // Arrange
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(
                () => this.offensiveWordsServiceProxy.GetOffensiveWordsList());
        }

        [Fact]
        public async Task GetOffensiveWordsList_ResponseContentIsNull_ReturnsEmptyHashSet()
        {
            // Arrange
            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("null", Encoding.UTF8, "application/json")
                });

            // Act
            var result = await this.offensiveWordsServiceProxy.GetOffensiveWordsList();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_DuplicateWords_ReturnsUniqueHashSet()
        {
            // Arrange
            var offensiveWords = new List<OffensiveWord>
            {
                new OffensiveWord("badword"),
                new OffensiveWord("BADWORD"), // Same word, different case
                new OffensiveWord("offensive"),
                new OffensiveWord("badword") // Duplicate
            };

            var jsonResponse = JsonSerializer.Serialize(offensiveWords);

            this.httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString() == $"{this.baseUrl}/api/offensiveWords"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await this.offensiveWordsServiceProxy.GetOffensiveWordsList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Should only contain unique words (case-insensitive)
            Assert.Contains("badword", result);
            Assert.Contains("offensive", result);
        }
    }
} 