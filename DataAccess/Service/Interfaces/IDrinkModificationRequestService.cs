using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Constants;
using DataAccess.Data;
using WinUiApp.Data.Data;

namespace DataAccess.Service.Interfaces
{
    public interface IDrinkModificationRequestService
    {
        DrinkModificationRequest AddRequest(DrinkModificationRequestType type, Drink? oldDrink, Drink? newDrink, User requestingUser);
        Task<IEnumerable<DrinkModificationRequest>> GetAllModificationRequests();
    }
}
