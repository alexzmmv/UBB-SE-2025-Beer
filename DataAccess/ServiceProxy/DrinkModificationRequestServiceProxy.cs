using DataAccess.Data;
using DataAccess.Service.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ServiceProxy
{
    public class DrinkModificationRequestServiceProxy : IDrinkModificationRequestService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl;
        private const string ApiBaseRoute = "api/DrinkModificationRequests";

        public DrinkModificationRequestServiceProxy(string baseUrl)
        {
            this.httpClient = new HttpClient();
            this.baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<IEnumerable<DrinkModificationRequest>> GetAllModificationRequests()
        {
            var response = await httpClient.GetAsync($"{baseUrl}/{ApiBaseRoute}/get-all");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<DrinkModificationRequest>>(json);
        }

        public async Task<DrinkModificationRequest> GetModificationRequest(int modificationRequestId)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/{ApiBaseRoute}?modificationRequestId={modificationRequestId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DrinkModificationRequest>(json);
        }

        public async Task DenyRequest(int modificationRequestId, Guid userId)
        {
            var jsonBody = JsonConvert.SerializeObject(userId);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(
                $"{baseUrl}/{ApiBaseRoute}/deny?modificationRequestId={modificationRequestId}",
                content
            );

            response.EnsureSuccessStatusCode();
        }

        public Task DenyRequest(int modificationRequestId)
        {
            throw new NotImplementedException();
        }
    }
}