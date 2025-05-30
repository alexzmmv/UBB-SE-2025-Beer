using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IDrinkModificationRequestRepository
    {
        Task<IEnumerable<DrinkModificationRequest>> GetAllModificationRequests();
        Task<DrinkModificationRequest> GetModificationRequest(int modificationRequestId);
        Task DeleteRequest(int modificationRequestId);
    }
}
