using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using DataAccess.DTOModels;

namespace DataAccess.IRepository
{
    public interface IDrinkModificationRequestRepository
    {
        DrinkModificationRequestDTO AddRequest(DrinkModificationRequestDTO request);

        Task<IEnumerable<DrinkModificationRequestDTO>> GetAllModificationRequests();

        Task<DrinkModificationRequestDTO> GetModificationRequest(int modificationRequestId);

        Task DeleteRequest(int modificationRequestId);
    }
}
