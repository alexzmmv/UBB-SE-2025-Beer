using System.Text;
using DataAccess.Constants;
using DataAccess.Data;
using DataAccess.DTOModels;
using DataAccess.Requests.Drinks;
using DataAccess.Service.Interfaces;
using Newtonsoft.Json;

namespace DataAccess.ServiceProxy
{
    public class DrinkModificationRequestServiceProxy : IDrinkModificationRequestService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl;
        private const string API_BASE_ROUTE = "api/DrinkModificationRequests";

        public DrinkModificationRequestServiceProxy(string baseUrl)
        {
            this.httpClient = new HttpClient();
            this.baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<IEnumerable<DrinkModificationRequestDTO>> GetAllModificationRequests()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{this.baseUrl}/{API_BASE_ROUTE}/get-all");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<DrinkModificationRequestDTO>>(json);
        }

        public async Task<DrinkModificationRequestDTO> GetModificationRequest(int modificationRequestId)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{this.baseUrl}/{API_BASE_ROUTE}?modificationRequestId={modificationRequestId}");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DrinkModificationRequestDTO>(json);
        }

        public async Task DenyRequest(int modificationRequestId, Guid userId)
        {
            DenyDrinkModificationRequest request = new DenyDrinkModificationRequest
            {
                ModificationRequestId = modificationRequestId,
                UserId = userId
            };

            string jsonBody = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await this.httpClient.PostAsync(
                $"{this.baseUrl}/{API_BASE_ROUTE}/deny",
                content
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task ApproveRequest(int modificationRequestId, Guid userId)
        {
            ApproveDrinkModificationRequest request = new ApproveDrinkModificationRequest
            {
                ModificationRequestId = modificationRequestId,
                UserId = userId
            };

            string jsonBody = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(
                $"{baseUrl}/{API_BASE_ROUTE}/approve",
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

            string jsonBody = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.PostAsync(
                $"{baseUrl}/{API_BASE_ROUTE}/add",
                content).Result;

            response.EnsureSuccessStatusCode();
            string responseJson = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<DrinkModificationRequestDTO>(responseJson);
        }
    }
}