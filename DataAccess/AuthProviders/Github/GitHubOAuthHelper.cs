using System.Diagnostics;
using System.Text.Json;
using DataAccess.OAuthProviders;

namespace DataAccess.AuthProviders.Github
{
    public class GitHubOAuthHelper : IGitHubOAuthHelper
    {
        private const string CLIENT_ID = "Ov23ligheYgI7JILPWGY";
        private const string CLIENT_SECRET = "791dfaf36750b2a34a752c4fe3fb3703cef18836";
        private const string REDIRECT_URI = "http://localhost:8890/auth";
        private const string SCOPE = "read:user user:email";
        private IGenericOAuth2Provider gitHubOAuth2Provider;
        private TaskCompletionSource<AuthenticationResponse> taskCompletionSource;
        private GitHubLocalOAuthServer localServer;
        private bool handlerExecuted = false;

        public GitHubOAuthHelper(IGenericOAuth2Provider gitHubOAuth2Provider, GitHubLocalOAuthServer localServer)
        {
            this.gitHubOAuth2Provider = gitHubOAuth2Provider;
            this.localServer = localServer;
            this.taskCompletionSource = new TaskCompletionSource<AuthenticationResponse>();
            GitHubLocalOAuthServer.OnCodeReceived += OnCodeReceived;
        }

        private string BuildAuthorizeUrl()
        {
            return $"https://github.com/login/oauth/authorize" +
                   $"?client_id={GitHubOAuthHelper.CLIENT_ID}" +
                   $"&redirect_uri={Uri.EscapeDataString(GitHubOAuthHelper.REDIRECT_URI)}" +
                   $"&scope={Uri.EscapeDataString(GitHubOAuthHelper.SCOPE)}" +
                   $"&response_type=code";
        }

        private async void OnCodeReceived(string code)
        {
            if (this.handlerExecuted)
            {
                return;
            }

            this.handlerExecuted = true;
            GitHubLocalOAuthServer.OnCodeReceived -= OnCodeReceived;

            if (this.taskCompletionSource == null || this.taskCompletionSource.Task.IsCompleted)
            {
                return;
            }

            try
            {
                string token = await ExchangeCodeForToken(code);
                AuthenticationResponse result = await gitHubOAuth2Provider.Authenticate(string.Empty, token);
                this.taskCompletionSource.SetResult(result);
            }
            catch (Exception exception)
            {
                this.taskCompletionSource.SetException(exception);
            }
        }

        public async Task<AuthenticationResponse> AuthenticateAsync()
        {
            this.handlerExecuted = false;
            GitHubLocalOAuthServer.OnCodeReceived -= OnCodeReceived;
            GitHubLocalOAuthServer.OnCodeReceived += OnCodeReceived;
            this.taskCompletionSource = new TaskCompletionSource<AuthenticationResponse>();

            Uri authorizeUri = new Uri(BuildAuthorizeUrl());
            Process.Start(new ProcessStartInfo
            {
                FileName = authorizeUri.ToString(),
                UseShellExecute = true
            });

            return await this.taskCompletionSource.Task;
        }

        private async Task<string> ExchangeCodeForToken(string code)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token");
                request.Headers.Add("Accept", "application/json"); // we want JSON response
                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", GitHubOAuthHelper.CLIENT_ID),
                    new KeyValuePair<string, string>("client_secret", GitHubOAuthHelper.CLIENT_SECRET),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", GitHubOAuthHelper.REDIRECT_URI)
                });
                request.Content = content;

                HttpResponseMessage response = await client.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                using JsonDocument responseDocument = JsonDocument.Parse(responseBody);
                if (responseDocument.RootElement.TryGetProperty("access_token", out JsonElement tokenProperty))
                {
                    return tokenProperty.GetString() ?? throw new Exception("Access token is null.");
                }
                throw new Exception("Failed to get access token from GitHub.");
            }
        }
    }
}
