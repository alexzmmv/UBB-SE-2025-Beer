using DataAccess.Data;
using DataAccess.Constants;
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
