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

namespace DataAccess.Service
{
    public class DrinkModificationRequestService : IDrinkModificationRequestService
    {
        private readonly IDrinkModificationRequestRepository drinkModificationRequestRepository;

        public DrinkModificationRequestService(IDrinkModificationRequestRepository drinkModificationRequestRepository)
        {
            this.drinkModificationRequestRepository = drinkModificationRequestRepository;
        }

        public DrinkModificationRequest AddRequest(DrinkModificationRequestType type, Drink? oldDrink, DrinkRequestingApproval? newDrink, User requestingUser)
        {
            DrinkModificationRequest request = new DrinkModificationRequest
        {
                ModificationType = type,
                OldDrink = oldDrink,
                NewDrink = newDrink,
                RequestingUser = requestingUser
            };

            this.drinkModificationRequestRepository.AddRequest(request);

            return request;
        }

        public async Task<IEnumerable<DrinkModificationRequest>> GetAllModificationRequests()
        {
            return await drinkModificationRequestRepository.GetAllModificationRequests();
        }
    }
}
