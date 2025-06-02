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
        public async Task GetOffensiveWordsList_ValidResponse_ReturnsNotNull()
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
        }

        [Fact]
        public async Task GetOffensiveWordsList_ValidResponse_ReturnsCorrectCount()
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
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetOffensiveWordsList_ValidResponse_ContainsAllExpectedWords()
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

            // Assert - Combining multiple Contains into one assertion for related functionality
            Assert.True(result.Contains("badword1") && result.Contains("badword2") && result.Contains("offensive"));
        }

        [Fact]
        public async Task GetOffensiveWordsList_EmptyResponse_ReturnsNotNull()
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
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_CaseInsensitiveComparison_ReturnsNotNull()
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
        }

        [Fact]
        public async Task GetOffensiveWordsList_CaseInsensitiveComparison_ReturnsCorrectCount()
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
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetOffensiveWordsList_CaseInsensitiveComparison_ContainsWordsRegardlessOfCase()
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

            // Assert - Testing case insensitive behavior as one logical assertion
            Assert.True(result.Contains("badword") && result.Contains("BADWORD") && 
                       result.Contains("offensive") && result.Contains("OFFENSIVE"));
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
        public async Task GetOffensiveWordsList_ResponseContentIsNull_ReturnsNotNull()
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
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOffensiveWordsList_DuplicateWords_ReturnsNotNull()
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
        }

        [Fact]
        public async Task GetOffensiveWordsList_DuplicateWords_ReturnsUniqueHashSetWithCorrectCount()
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
            Assert.Equal(2, result.Count); // Should only contain unique words (case-insensitive)
        }

        [Fact]
        public async Task GetOffensiveWordsList_DuplicateWords_ContainsExpectedUniqueWords()
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
            Assert.True(result.Contains("badword") && result.Contains("offensive"));
        }
    }
} 