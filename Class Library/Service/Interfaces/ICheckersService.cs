namespace DataAccess.Service.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Model.AdminDashboard;
    using WinUiApp.Data.Data;

    public interface ICheckersService
    {
        public Task<List<string>> RunAutoCheck(List<Review> reviews);

        public Task<HashSet<string>> GetOffensiveWordsList();

        public Task AddOffensiveWordAsync(string newWord);

        public Task DeleteOffensiveWordAsync(string word);

        public void RunAICheckForOneReviewAsync(Review review);
    }
}
