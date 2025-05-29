using DataAccess.Data;
using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.IRepository;
using WinUiApp.Data.Interfaces;

namespace DataAccess.Repository
{
    public class DrinkModificationRequestRepository : IDrinkModificationRequestRepository
    {
        private readonly IAppDbContext dbContext;

        public DrinkModificationRequestRepository(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DrinkModificationRequest AddRequest(DrinkModificationRequest request)
        {
            this.dbContext.DrinkModificationRequests.Add(request);
            this.dbContext.SaveChanges();

            return request;
        }

        public async Task<IEnumerable<DrinkModificationRequest>> GetAllModificationRequests()
        {
            return await dbContext.DrinkModificationRequests
                .Include(drink => drink.OldDrink)
                .Include(drink => drink.NewDrink)
                .ToListAsync();
        }
    }
}
