using DataAccess.Data;
using DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUiApp.Data.Interfaces;

namespace DataAccess.Repository
{
    public class DrinkModificationRequestRepository: IDrinkModificationRequestRepository
    {
        IAppDbContext dbContext;
        public DrinkModificationRequestRepository(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
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
