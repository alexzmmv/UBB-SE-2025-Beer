using DataAccess.Constants;
using DataAccess.Data;
using DataAccess.DTOModels;
using DataAccess.Requests.Drinks;
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

        public async Task<IEnumerable<DrinkModificationRequestDTO>> GetAllModificationRequests()
        {
            var response = await httpClient.GetAsync($"{baseUrl}/{ApiBaseRoute}/get-all");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<DrinkModificationRequestDTO>>(json);
        }

        public async Task<DrinkModificationRequestDTO> GetModificationRequest(int modificationRequestId)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/{ApiBaseRoute}?modificationRequestId={modificationRequestId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DrinkModificationRequestDTO>(json);
        }

        public async Task DenyRequest(int modificationRequestId, Guid userId)
        {
            var request = new DenyDrinkModificationRequest
            {
                ModificationRequestId = modificationRequestId,
                userId = userId
            };

            var jsonBody = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(
                $"{baseUrl}/{ApiBaseRoute}/deny",
                content
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task ApproveRequest(int modificationRequestId, Guid userId)
        {
            var request = new ApproveDrinkModificationRequest
            {
                ModificationRequestId = modificationRequestId,
                userId = userId
            };

            var jsonBody = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(
                $"{baseUrl}/{ApiBaseRoute}/approve",
                content
            );

            response.EnsureSuccessStatusCode();
        }

        public DrinkModificationRequestDTO AddRequest(DrinkModificationRequestType type, int? oldDrinkId, int? newDrinkId, Guid requestingUserId)
        {
            var request = new
            {
                ModificationType = type,
                OldDrinkId = oldDrinkId,
                NewDrinkId = newDrinkId,
                RequestingUserId = requestingUserId
            };

            var jsonBody = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(
                $"{baseUrl}/{ApiBaseRoute}/add",
                content
            ).Result;

            response.EnsureSuccessStatusCode();
            var responseJson = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<DrinkModificationRequestDTO>(responseJson);
        }
    }
}