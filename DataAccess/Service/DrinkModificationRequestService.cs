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
using DataAccess.Constants;
using DataAccess.Data;
using DataAccess.IRepository;
using DataAccess.Service.Interfaces;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Models;
using DataAccess.DTOModels;

namespace DataAccess.Service
{
    public class DrinkModificationRequestService: IDrinkModificationRequestService
    {
        private readonly IDrinkModificationRequestRepository drinkModificationRequestRepository;
        private readonly IDrinkRepository drinkRepository;

        public DrinkModificationRequestService(IDrinkModificationRequestRepository drinkModificationRequestRepository, IDrinkRepository drinkRepository)
        {
            this.drinkModificationRequestRepository = drinkModificationRequestRepository;
            this.drinkRepository = drinkRepository;
        }

        public DrinkModificationRequestDTO AddRequest(DrinkModificationRequestType type, int? oldDrinkId, int? newDrinkId, Guid requestingUserId)
        {
            DrinkModificationRequestDTO request = new()
            {
                ModificationType = type,
                OldDrinkId = oldDrinkId,
                NewDrinkId = newDrinkId,
                RequestingUserId = requestingUserId
            };

            this.drinkModificationRequestRepository.AddRequest(request);

            return request;
        }

        public async Task<IEnumerable<DrinkModificationRequestDTO>> GetAllModificationRequests()
        {
            return await this.drinkModificationRequestRepository.GetAllModificationRequests();
        }

        public async Task<DrinkModificationRequestDTO> GetModificationRequest(int modificationRequestId)
        {
            return await this.drinkModificationRequestRepository.GetModificationRequest(modificationRequestId);
        }

        public async Task DenyRequest(int modificationRequestId)
        {
            var modificationRequest = await drinkModificationRequestRepository
                .GetModificationRequest(modificationRequestId) ?? throw new InvalidOperationException($"Modification request {modificationRequestId} not found.");

            await drinkModificationRequestRepository.DeleteRequest(modificationRequestId);

            if (modificationRequest.NewDrinkId.HasValue)
            {
                drinkRepository.DeleteDrink(modificationRequest.NewDrinkId.Value);
            }
        }

    }
}
