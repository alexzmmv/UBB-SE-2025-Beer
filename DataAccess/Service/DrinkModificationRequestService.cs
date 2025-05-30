using DataAccess.Constants;
using DataAccess.Data;
using DataAccess.IRepository;
using DataAccess.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUiApp.Data.Interfaces;

namespace DataAccess.Service
{
    public class DrinkModificationRequestService: IDrinkModificationRequestService
    {
        IDrinkModificationRequestRepository drinkModificationRepository;
        IDrinkRepository drinkRepository;

        public DrinkModificationRequestService(IDrinkModificationRequestRepository drinkModificationRepository, IDrinkRepository drinkRepository)
        {
            this.drinkModificationRepository = drinkModificationRepository;
            this.drinkRepository = drinkRepository;
        }

        public async Task<IEnumerable<DrinkModificationRequest>> GetAllModificationRequests()
        {
            return await drinkModificationRepository.GetAllModificationRequests();
        }

        public async Task<DrinkModificationRequest> GetModificationRequest(int modificationRequestId)
        {
            return await drinkModificationRepository.GetModificationRequest(modificationRequestId);
        }

        public async Task DenyRequest(int modificationRequestId)
        {
            var modificationRequest = await drinkModificationRepository.GetModificationRequest(modificationRequestId);
            drinkRepository.DeleteRequestingApprovalDrink(modificationRequest.NewDrink.DrinkId);

            await drinkModificationRepository.DeleteRequest(modificationRequestId);
        }
    }
}
