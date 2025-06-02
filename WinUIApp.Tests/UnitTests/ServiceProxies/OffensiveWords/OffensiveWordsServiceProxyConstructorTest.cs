using System;
using DataAccess.ServiceProxy;

namespace WinUIApp.Tests.UnitTests.ServiceProxies.OffensiveWords
{
    public class OffensiveWordsServiceProxyConstructorTest
    {
        [Fact]
        public void Constructor_ValidBaseUrl_InitializesProxyNotNull()
        {
            // Arrange
            string baseUrl = "https://api.example.com";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrl);

            // Assert
            Assert.NotNull(proxy);
        }

        [Fact]
        public void Constructor_ValidBaseUrl_SetsBaseUrlFieldCorrectly()
        {
            // Arrange
            string baseUrl = "https://api.example.com";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrl);

            // Assert
            var baseUrlField = typeof(OffensiveWordsServiceProxy).GetField("baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualBaseUrl = baseUrlField?.GetValue(proxy) as string;
            Assert.Equal("https://api.example.com", actualBaseUrl);
        }

        [Fact]
        public void Constructor_ValidBaseUrl_InitializesHttpClientField()
        {
            // Arrange
            string baseUrl = "https://api.example.com";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrl);

            // Assert
            var httpClientField = typeof(OffensiveWordsServiceProxy).GetField("httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var httpClient = httpClientField?.GetValue(proxy);
            Assert.NotNull(httpClient);
        }

        [Fact]
        public void Constructor_BaseUrlWithTrailingSlash_InitializesProxy()
        {
            // Arrange
            string baseUrlWithSlash = "https://api.example.com/";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrlWithSlash);

            // Assert
            Assert.NotNull(proxy);
        }

        [Fact]
        public void Constructor_BaseUrlWithTrailingSlash_TrimsSlash()
        {
            // Arrange
            string baseUrlWithSlash = "https://api.example.com/";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrlWithSlash);

            // Assert
            var baseUrlField = typeof(OffensiveWordsServiceProxy).GetField("baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualBaseUrl = baseUrlField?.GetValue(proxy) as string;
            Assert.Equal("https://api.example.com", actualBaseUrl);
        }

        [Fact]
        public void Constructor_BaseUrlWithMultipleTrailingSlashes_InitializesProxy()
        {
            // Arrange
            string baseUrlWithSlashes = "https://api.example.com///";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrlWithSlashes);

            // Assert
            Assert.NotNull(proxy);
        }

        [Fact]
        public void Constructor_BaseUrlWithMultipleTrailingSlashes_TrimsAllSlashes()
        {
            // Arrange
            string baseUrlWithSlashes = "https://api.example.com///";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrlWithSlashes);

            // Assert
            var baseUrlField = typeof(OffensiveWordsServiceProxy).GetField("baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualBaseUrl = baseUrlField?.GetValue(proxy) as string;
            Assert.Equal("https://api.example.com", actualBaseUrl);
        }

        [Fact]
        public void Constructor_EmptyBaseUrl_InitializesProxy()
        {
            // Arrange
            string emptyBaseUrl = string.Empty;

            // Act
            var proxy = new OffensiveWordsServiceProxy(emptyBaseUrl);

            // Assert
            Assert.NotNull(proxy);
        }

        [Fact]
        public void Constructor_EmptyBaseUrl_SetsEmptyBaseUrlField()
        {
            // Arrange
            string emptyBaseUrl = string.Empty;

            // Act
            var proxy = new OffensiveWordsServiceProxy(emptyBaseUrl);

            // Assert
            var baseUrlField = typeof(OffensiveWordsServiceProxy).GetField("baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualBaseUrl = baseUrlField?.GetValue(proxy) as string;
            Assert.Equal(string.Empty, actualBaseUrl);
        }

        [Fact]
        public void Constructor_NullBaseUrl_ThrowsException()
        {
            // Arrange
            string nullBaseUrl = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => new OffensiveWordsServiceProxy(nullBaseUrl));
        }

        [Fact]
        public void Constructor_BaseUrlWithPath_InitializesProxy()
        {
            // Arrange
            string baseUrlWithPath = "https://api.example.com/v1/services";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrlWithPath);

            // Assert
            Assert.NotNull(proxy);
        }

        [Fact]
        public void Constructor_BaseUrlWithPath_KeepsPath()
        {
            // Arrange
            string baseUrlWithPath = "https://api.example.com/v1/services";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrlWithPath);

            // Assert
            var baseUrlField = typeof(OffensiveWordsServiceProxy).GetField("baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualBaseUrl = baseUrlField?.GetValue(proxy) as string;
            Assert.Equal("https://api.example.com/v1/services", actualBaseUrl);
        }

        [Fact]
        public void Constructor_BaseUrlWithPathAndTrailingSlash_InitializesProxy()
        {
            // Arrange
            string baseUrlWithPathAndSlash = "https://api.example.com/v1/services/";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrlWithPathAndSlash);

            // Assert
            Assert.NotNull(proxy);
        }

        [Fact]
        public void Constructor_BaseUrlWithPathAndTrailingSlash_TrimsSlashButKeepsPath()
        {
            // Arrange
            string baseUrlWithPathAndSlash = "https://api.example.com/v1/services/";

            // Act
            var proxy = new OffensiveWordsServiceProxy(baseUrlWithPathAndSlash);

            // Assert
            var baseUrlField = typeof(OffensiveWordsServiceProxy).GetField("baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualBaseUrl = baseUrlField?.GetValue(proxy) as string;
            Assert.Equal("https://api.example.com/v1/services", actualBaseUrl);
        }

        [Fact]
        public void Constructor_LocalhostUrl_InitializesProxy()
        {
            // Arrange
            string localhostUrl = "http://localhost:8080";

            // Act
            var proxy = new OffensiveWordsServiceProxy(localhostUrl);

            // Assert
            Assert.NotNull(proxy);
        }

        [Fact]
        public void Constructor_LocalhostUrl_SetsBaseUrlCorrectly()
        {
            // Arrange
            string localhostUrl = "http://localhost:8080";

            // Act
            var proxy = new OffensiveWordsServiceProxy(localhostUrl);

            // Assert
            var baseUrlField = typeof(OffensiveWordsServiceProxy).GetField("baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualBaseUrl = baseUrlField?.GetValue(proxy) as string;
            Assert.Equal("http://localhost:8080", actualBaseUrl);
        }

        [Fact]
        public void Constructor_HttpsUrl_InitializesProxy()
        {
            // Arrange
            string httpsUrl = "https://secure-api.example.com";

            // Act
            var proxy = new OffensiveWordsServiceProxy(httpsUrl);

            // Assert
            Assert.NotNull(proxy);
        }

        [Fact]
        public void Constructor_HttpsUrl_SetsBaseUrlCorrectly()
        {
            // Arrange
            string httpsUrl = "https://secure-api.example.com";

            // Act
            var proxy = new OffensiveWordsServiceProxy(httpsUrl);

            // Assert
            var baseUrlField = typeof(OffensiveWordsServiceProxy).GetField("baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualBaseUrl = baseUrlField?.GetValue(proxy) as string;
            Assert.Equal("https://secure-api.example.com", actualBaseUrl);
        }

        [Fact]
        public void Constructor_UrlWithQueryParameters_InitializesProxy()
        {
            // Arrange
            string urlWithQuery = "https://api.example.com?version=v1&format=json";

            // Act
            var proxy = new OffensiveWordsServiceProxy(urlWithQuery);

            // Assert
            Assert.NotNull(proxy);
        }

        [Fact]
        public void Constructor_UrlWithQueryParameters_KeepsQueryParameters()
        {
            // Arrange
            string urlWithQuery = "https://api.example.com?version=v1&format=json";

            // Act
            var proxy = new OffensiveWordsServiceProxy(urlWithQuery);

            // Assert
            var baseUrlField = typeof(OffensiveWordsServiceProxy).GetField("baseUrl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var actualBaseUrl = baseUrlField?.GetValue(proxy) as string;
            Assert.Equal("https://api.example.com?version=v1&format=json", actualBaseUrl);
        }
    }
} 