using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data;

namespace DataAccess.IRepository
{
    public interface IDrinkModificationRequestRepository
    {
        DrinkModificationRequest AddRequest(DrinkModificationRequest request);

        Task<IEnumerable<DrinkModificationRequest>> GetAllModificationRequests();
    }
}
