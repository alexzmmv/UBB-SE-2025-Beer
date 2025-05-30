using DataAccess.AutoChecker;
using DataAccess.Data;
using DataAccess.DTOModels;
using DataAccess.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/DrinkModificationRequests")]
    public class DrinkModificationRequestsController
    {
        IDrinkModificationRequestService drinkModificationService;

        public DrinkModificationRequestsController(IDrinkModificationRequestService drinkModificationService)
        {
            this.drinkModificationService = drinkModificationService;
        }

        [HttpGet("get-all")]
        public async Task<IEnumerable<DrinkModificationRequestDTO>> GetAll()
        {
            return await this.drinkModificationService.GetAllModificationRequests();
        }
    }
}
