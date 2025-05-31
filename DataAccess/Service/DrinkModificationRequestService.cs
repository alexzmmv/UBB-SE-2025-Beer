using DataAccess.Constants;
using DataAccess.Data;
using DataAccess.IRepository;
using DataAccess.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using WinUiApp.Data.Data;
using WinUiApp.Data.Interfaces;
using WinUIApp.WebAPI.Models;
using DataAccess.DTOModels;

namespace DataAccess.Service
{
    public class DrinkModificationRequestService: IDrinkModificationRequestService
    {
        private readonly IDrinkModificationRequestRepository drinkModificationRequestRepository;
        private readonly IDrinkRepository drinkRepository;

        public DrinkModificationRequestService(IDrinkModificationRequestRepository drinkModificationRequestRepository, IDrinkRepository drinkRepository)
        {
            this.drinkModificationRequestRepository = drinkModificationRequestRepository;
            this.drinkRepository = drinkRepository;
        }

        public DrinkModificationRequestDTO AddRequest(DrinkModificationRequestType type, int? oldDrinkId, int? newDrinkId, Guid requestingUserId)
        {
            DrinkModificationRequestDTO request = new ()
            {
                ModificationType = type,
                OldDrinkId = oldDrinkId,
                NewDrinkId = newDrinkId,
                RequestingUserId = requestingUserId
            };

            this.drinkModificationRequestRepository.AddRequest(request);

            return request;
        }

        public async Task<IEnumerable<DrinkModificationRequestDTO>> GetAllModificationRequests()
        {
            return await this.drinkModificationRequestRepository.GetAllModificationRequests();
        }

        public async Task<DrinkModificationRequestDTO> GetModificationRequest(int modificationRequestId)
        {
            return await this.drinkModificationRequestRepository.GetModificationRequest(modificationRequestId);
        }

        public async Task DenyRequest(int modificationRequestId, Guid userId)
        {
            DrinkModificationRequestDTO modificationRequest = await this.drinkModificationRequestRepository
                .GetModificationRequest(modificationRequestId) ?? throw new InvalidOperationException($"Modification request {modificationRequestId} not found.");

            await this.drinkModificationRequestRepository.DeleteRequest(modificationRequestId);

            if (modificationRequest.NewDrinkId.HasValue)
            {
                this.drinkRepository.DeleteDrink(modificationRequest.NewDrinkId.Value);
            }
        }

        public async Task ApproveRequest(int modificationRequestId, Guid userId)
        {
            DrinkModificationRequestDTO request = await this.drinkModificationRequestRepository
                .GetModificationRequest(modificationRequestId) ?? throw new InvalidOperationException($"Modification request {modificationRequestId} not found.");

            switch (request.ModificationType)
            {
                case DrinkModificationRequestType.Add:
                    DrinkDTO? newDrink = this.drinkRepository.GetDrinkById(request.NewDrinkId.Value);

                    newDrink.IsRequestingApproval = false;
                    this.drinkRepository.UpdateDrink(newDrink);
                    break;

                case DrinkModificationRequestType.Edit:
                    DrinkDTO? oldDrink = this.drinkRepository.GetDrinkById(request.OldDrinkId.Value);
                    DrinkDTO? updatedDrink = this.drinkRepository.GetDrinkById(request.NewDrinkId.Value);

                    oldDrink.DrinkName = updatedDrink.DrinkName;
                    oldDrink.DrinkImageUrl = updatedDrink.DrinkImageUrl;
                    oldDrink.CategoryList = updatedDrink.CategoryList;
                    oldDrink.DrinkBrand = updatedDrink.DrinkBrand;
                    oldDrink.AlcoholContent = updatedDrink.AlcoholContent;

                    this.drinkRepository.UpdateDrink(oldDrink);
                    this.drinkRepository.DeleteDrink(request.NewDrinkId.Value);
                    break;

                case DrinkModificationRequestType.Remove:
                    this.drinkRepository.DeleteDrink(request.OldDrinkId.Value);
                    break;

                default:
                    throw new InvalidOperationException($"Unknown modification type: {request.ModificationType}");
            }

            await this.drinkModificationRequestRepository.DeleteRequest(modificationRequestId);
        }
    }
}
