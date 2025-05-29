using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service.Interfaces
{
    public interface IDrinkModificationRequestService
    {
        Task<IEnumerable<DrinkModificationRequest>> GetAllModificationRequests();
    }
}
