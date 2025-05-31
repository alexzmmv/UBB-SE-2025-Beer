using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model.Authentication;
using DataAccess.OAuthProviders;
using DataAccess.Service;
using DataAccess.Service.Interfaces;
using Newtonsoft.Json;
using WinUiApp.Data.Data;

namespace DataAccess.ServiceProxy
{
    public class AuthenticationServiceProxy : IAuthenticationService
    {
        private readonly HttpClient httpClient;
        private const string API_BASE_ROUTE = "api/auth";

        public AuthenticationServiceProxy(string baseUrl)
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<AuthenticationResponse> AuthWithUserPass(string username, string password)
        {
            StringContent content = new StringContent(
                JsonConvert.SerializeObject(new { Username = username, Password = password }),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await this.httpClient.PostAsync($"{API_BASE_ROUTE}/login", content);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            AuthenticationResponse? authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(json);
            if (authenticationResponse == null)
            {
                return new AuthenticationResponse { AuthenticationSuccessful = false, NewAccount = false, OAuthToken = null, SessionId = Guid.Empty };
            }
            return authenticationResponse;
        }

        public async Task<AuthenticationResponse> AuthWithOAuth(OAuthService selectedService, object authProvider)
        {
            HttpResponseMessage response = await this.httpClient.PostAsync($"{API_BASE_ROUTE}/oauth?service={selectedService}", null);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
           AuthenticationResponse? authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(json);
            if (authenticationResponse == null)
            {
                return new AuthenticationResponse { AuthenticationSuccessful = false, NewAccount = false, OAuthToken = null, SessionId = Guid.Empty };
            }
            return authenticationResponse;
        }

        public async Task<User?> GetUser(Guid sessionId)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"api/auth/user?sessionId={sessionId}");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<User>(json);
        }

        public async void Logout()
        {
            HttpResponseMessage response = await this.httpClient.PostAsync($"{API_BASE_ROUTE}/logout", null);
            response.EnsureSuccessStatusCode();
        }
    }
}