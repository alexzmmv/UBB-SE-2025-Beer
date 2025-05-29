namespace DataAccess.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Model.AdminDashboard;
    using IRepository;
    using WinUiApp.Data;
    using WinUiApp.Data.Interfaces;

    public class OffensiveWordsRepository : IOffensiveWordsRepository
    {
        private IAppDbContext databaseContext;

        public OffensiveWordsRepository(IAppDbContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<HashSet<string>> LoadOffensiveWords()
        {
            return await Task.FromResult(databaseContext.OffensiveWords
                .Select(w => w.Word)
                .ToHashSet(StringComparer.OrdinalIgnoreCase));
        }

        public async Task AddWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }

            bool exists = databaseContext.OffensiveWords.Any(item => item.Word == word);

            if (!exists)
            {
                databaseContext.OffensiveWords.Add(new OffensiveWord { Word = word });
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task DeleteWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }

            OffensiveWord? item = databaseContext.OffensiveWords.FirstOrDefault(item => item.Word == word);

            if (item == null)
            {
                return;
            }

            databaseContext.OffensiveWords.Remove(item);
            await databaseContext.SaveChangesAsync();
        }
    }
}
