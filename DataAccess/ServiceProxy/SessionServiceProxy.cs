using System;
using System.Net.Http;
using System.Threading.Tasks;
using DataAccess.Model.Authentication;
using DataAccess.Service.Interfaces;
using Newtonsoft.Json;

namespace DataAccess.ServiceProxy
{
    public class SessionServiceProxy : ISessionService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl;
        private const string API_BASE_ROUTE = "api/sessions";

        public SessionServiceProxy(string baseUrl)
        {
            this.httpClient = new HttpClient();
            this.baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<Session?> CreateSessionAsync(Guid userId)
        {
            HttpResponseMessage response = await this.httpClient.PostAsync($"{this.baseUrl}/{API_BASE_ROUTE}/add?userId={userId}", null);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Session>(json);
        }

        public async Task<bool> EndSessionAsync(Guid sessionId)
        {
            HttpResponseMessage response = await this.httpClient.DeleteAsync($"{this.baseUrl}/{API_BASE_ROUTE}/{sessionId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Session?> GetSessionAsync(Guid sessionId)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{this.baseUrl}/{API_BASE_ROUTE}/{sessionId}");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Session>(json);
        }

        public async Task<Session?> GetSessionByUserIdAsync(Guid userId)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{this.baseUrl}/{API_BASE_ROUTE}/by-user/{userId}");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Session>(json);
        }

        public async Task<bool> AuthorizeActionAsync(Guid sessionId, string resource, string action)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{this.baseUrl}/{API_BASE_ROUTE}/{sessionId}/authorize?resource={resource}&action={action}");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(json);
        }
    }
}