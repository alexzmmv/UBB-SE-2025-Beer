using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service.Interfaces;
using Newtonsoft.Json;

namespace DataAccess.ServiceProxy
{
    public class UpgradeRequestsServiceProxy : IUpgradeRequestsService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl;
        private const string ApiBaseRoute = "api/upgradeRequests";

        public UpgradeRequestsServiceProxy(string baseUrl)
        {
            httpClient = new HttpClient();
            this.baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<List<UpgradeRequest>> RetrieveAllUpgradeRequests()
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/{ApiBaseRoute}");
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

            HttpResponseMessage response = await httpClient.PostAsync(
                $"{baseUrl}/{ApiBaseRoute}/{upgradeRequestIdentifier}/process",
                content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> GetRoleNameBasedOnIdentifier(RoleType roleType)
        {
            return await Task.FromResult(roleType.ToString());
        }

        public async Task RemoveUpgradeRequestByIdentifier(int upgradeRequestIdentifier)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"{baseUrl}/{ApiBaseRoute}/{upgradeRequestIdentifier}/delete");
            response.EnsureSuccessStatusCode();
        }

        public async Task<UpgradeRequest?> RetrieveUpgradeRequestByIdentifier(int upgradeRequestIdentifier)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/{ApiBaseRoute}/{upgradeRequestIdentifier}");
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

            HttpResponseMessage response = await httpClient.PostAsync($"{baseUrl}/{ApiBaseRoute}/add", content);
            response.EnsureSuccessStatusCode();
        }
    }
}