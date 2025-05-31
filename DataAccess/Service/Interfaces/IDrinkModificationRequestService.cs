using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Constants;
using DataAccess.Data;
using WinUiApp.Data.Data;
using DataAccess.DTOModels;
using WinUIApp.WebAPI.Models;

namespace DataAccess.Service.Interfaces
{
    public interface IDrinkModificationRequestService
    {
        DrinkModificationRequestDTO AddRequest(DrinkModificationRequestType type, int? oldDrinkId, int? newDrinkId, Guid requestingUserId);
        Task<IEnumerable<DrinkModificationRequestDTO>> GetAllModificationRequests();
        Task<DrinkModificationRequestDTO> GetModificationRequest(int modificationRequestId);
        Task DenyRequest(int modificationRequestId, Guid userId);
        Task ApproveRequest(int modificationRequestId, Guid userId);
    }
}
