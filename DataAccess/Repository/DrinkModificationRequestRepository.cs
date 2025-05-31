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
using DataAccess.DTOModels;
using DataAccess.Extensions;

namespace DataAccess.Repository
{
    public class DrinkModificationRequestRepository : IDrinkModificationRequestRepository
    {
        private readonly IAppDbContext dbContext;

        public DrinkModificationRequestRepository(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DrinkModificationRequestDTO AddRequest(DrinkModificationRequestDTO requestDTO)
        {
            this.dbContext.DrinkModificationRequests.Add(DrinkModificationRequestExtensions.ConvertDTOToEntity(requestDTO));
            this.dbContext.SaveChanges();

            return requestDTO;
        }

        public async Task<IEnumerable<DrinkModificationRequestDTO>> GetAllModificationRequests()
        {
            List<DrinkModificationRequest> requests = await dbContext.DrinkModificationRequests
                .Include(drink => drink.OldDrink)
                .Include(drink => drink.NewDrink)
                .ToListAsync();

            return requests.Select((request) =>
            {
                return DrinkModificationRequestExtensions.ConvertEntityToDTO(request);
            });
        }

        public async Task<DrinkModificationRequestDTO> GetModificationRequest(int modificationRequestId)
        {
            DrinkModificationRequest request = await dbContext.DrinkModificationRequests
                .Where(drink => drink.DrinkModificationRequestId == modificationRequestId)
                .FirstOrDefaultAsync() ?? throw new Exception("No request with given id");

            return DrinkModificationRequestExtensions.ConvertEntityToDTO(request);
        }

        // this should ideally return the request object back
        public async Task DeleteRequest(int modificationRequestId)
        {
            var request = await dbContext.DrinkModificationRequests
                .FirstOrDefaultAsync(drink => drink.DrinkModificationRequestId == modificationRequestId);

            if (request != null)
            {
                dbContext.DrinkModificationRequests.Remove(request);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
