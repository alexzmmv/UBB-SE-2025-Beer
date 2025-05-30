using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.DTOModels;

namespace DataAccess.IRepository
{
    public interface IDrinkModificationRequestRepository
    {
        DrinkModificationRequestDTO AddRequest(DrinkModificationRequestDTO request);

        Task<IEnumerable<DrinkModificationRequestDTO>> GetAllModificationRequests();
        Task DeleteRequest(int modificationRequestId);
    }
}
