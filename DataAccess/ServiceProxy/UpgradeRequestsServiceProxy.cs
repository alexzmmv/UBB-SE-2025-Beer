using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Constants;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service.Interfaces;
using Newtonsoft.Json;

namespace DataAccess.ServiceProxy
{
    public class UpgradeRequestsServiceProxy : IUpgradeRequestsService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl;
        private const string API_BASE_ROUTE = "api/upgradeRequests";

        public UpgradeRequestsServiceProxy(string baseUrl)
        {
            this.httpClient = new HttpClient();
            this.baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<List<UpgradeRequest>> RetrieveAllUpgradeRequests()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{this.baseUrl}/{API_BASE_ROUTE}");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<UpgradeRequest>>(json) ?? new List<UpgradeRequest>();
        }

        public async Task ProcessUpgradeRequest(bool isRequestAccepted, int upgradeRequestIdentifier)
        {
            StringContent content = new StringContent(
                JsonConvert.SerializeObject(isRequestAccepted),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await this.httpClient.PostAsync(
                $"{this.baseUrl}/{API_BASE_ROUTE}/{upgradeRequestIdentifier}/process",
                content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> GetRoleNameBasedOnIdentifier(RoleType roleType)
        {
            return await Task.FromResult(roleType.ToString());
        }

        public async Task RemoveUpgradeRequestByIdentifier(int upgradeRequestIdentifier)
        {
            HttpResponseMessage response = await this.httpClient.DeleteAsync($"{this.baseUrl}/{API_BASE_ROUTE}/{upgradeRequestIdentifier}/delete");
            response.EnsureSuccessStatusCode();
        }

        public async Task<UpgradeRequest?> RetrieveUpgradeRequestByIdentifier(int upgradeRequestIdentifier)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{this.baseUrl}/{API_BASE_ROUTE}/{upgradeRequestIdentifier}");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UpgradeRequest>(json);
        }

        public async Task AddUpgradeRequest(Guid userId)
        {
            StringContent content = new StringContent(
                JsonConvert.SerializeObject(userId),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await this.httpClient.PostAsync($"{this.baseUrl}/{API_BASE_ROUTE}/add", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> HasPendingUpgradeRequest(Guid userId)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/{API_BASE_ROUTE}/user/{userId}/hasPending");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(json);
            }
            catch (HttpRequestException)
            {
                // Return false if API call fails (network issues, server down, etc.)
                return false;
            }
            catch (Exception)
            {
                // Return false for any other errors (deserialization, etc.)
                return false;
            }
        }
    }
}