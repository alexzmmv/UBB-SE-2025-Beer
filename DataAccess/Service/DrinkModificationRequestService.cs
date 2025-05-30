using DataAccess.Data;
using DataAccess.IRepository;
using DataAccess.Service.Interfaces;
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
    public class DrinkModificationRequestService : IDrinkModificationRequestService
    {
        private readonly IDrinkModificationRequestRepository drinkModificationRequestRepository;

        public DrinkModificationRequestService(IDrinkModificationRequestRepository drinkModificationRequestRepository)
        {
            this.drinkModificationRequestRepository = drinkModificationRequestRepository;
        }

        public DrinkModificationRequestDTO AddRequest(DrinkModificationRequestType type, int? oldDrinkId, int? newDrinkId, Guid requestingUserId)
        {
            DrinkModificationRequestDTO request = new ()
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
            return await drinkModificationRequestRepository.GetAllModificationRequests();
        }
    }
}
