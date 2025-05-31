using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service.Interfaces;
using WinUiApp.Data.Data;

namespace DataAccess.ServiceProxy
{
    public class OffensiveWordsServiceProxy : ICheckersService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUrl;
        private const string API_BASE_ROUTE = "api/offensiveWords";

        public OffensiveWordsServiceProxy(string baseUrl)
        {
            this.httpClient = new HttpClient();
            this.baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<List<string>> RunAutoCheck(List<Review> reviews)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync($"{this.baseUrl}/{API_BASE_ROUTE}/check", reviews);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<HashSet<string>> GetOffensiveWordsList()
        {
            return await LoadOffensiveWords();
        }

        public async Task AddOffensiveWordAsync(string newWord)
        {
            if (string.IsNullOrWhiteSpace(newWord))
            {
                return;
            }
            await AddWord(newWord);
        }

        public async Task DeleteOffensiveWordAsync(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }
            await DeleteWord(word);
        }

        public async void RunAICheckForOneReviewAsync(Review review)
        {
            if (review?.Content == null)
            {
                return;
            }

            try
            {
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync($"{this.baseUrl}/{API_BASE_ROUTE}/checkOne", review);
                response.EnsureSuccessStatusCode();
            }
            catch
            {
            }
        }

        private async Task AddWord(string word)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{this.baseUrl}/{API_BASE_ROUTE}");
            response.EnsureSuccessStatusCode();

            List<OffensiveWord> offensiveWords = await response.Content.ReadFromJsonAsync<List<OffensiveWord>>() ?? new List<OffensiveWord>();

            bool wordExists = false;
            foreach (OffensiveWord offensive in offensiveWords)
            {
                if (offensive.Word == word)
                {
                    wordExists = true;
                    break;
                }
            }

            if (!wordExists)
            {
                HttpResponseMessage postResponse = await this.httpClient.PostAsJsonAsync($"{this.baseUrl}/{API_BASE_ROUTE}/add", new OffensiveWord { Word = word });
                postResponse.EnsureSuccessStatusCode();
            }
        }

        private async Task DeleteWord(string word)
        {
            HttpResponseMessage response = await this.httpClient.DeleteAsync($"{this.baseUrl}/{API_BASE_ROUTE}/delete/{Uri.EscapeDataString(word)}");
            response.EnsureSuccessStatusCode();
        }

        private async Task<HashSet<string>> LoadOffensiveWords()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{this.baseUrl}/{API_BASE_ROUTE}");
            response.EnsureSuccessStatusCode();

            List<OffensiveWord> offensiveWords = await response.Content.ReadFromJsonAsync<List<OffensiveWord>>() ?? new List<OffensiveWord>();
            return new HashSet<string>(offensiveWords.Select(w => w.Word), StringComparer.OrdinalIgnoreCase);
        }
    }
}