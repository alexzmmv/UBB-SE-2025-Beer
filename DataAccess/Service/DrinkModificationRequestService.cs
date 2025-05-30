using DataAccess.Data;
using DataAccess.IRepository;
using DataAccess.Service.Interfaces;
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
        public DrinkModificationRequestService(IDrinkModificationRequestRepository drinkModificationRepository)
        {
            this.drinkModificationRepository = drinkModificationRepository;
        }

        public async Task<IEnumerable<DrinkModificationRequest>> GetAllModificationRequests()
        {
            return await drinkModificationRepository.GetAllModificationRequests();
        }
    }
}
