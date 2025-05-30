using DataAccess.AutoChecker;
using DataAccess.Constants;
using DataAccess.Data;
using DataAccess.Service;
using DataAccess.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/DrinkModificationRequests")]
    public class DrinkModificationRequestsController
    {
        private readonly IDrinkModificationRequestService drinkModificationService;
        private readonly IUserService userService;

        public DrinkModificationRequestsController(IDrinkModificationRequestService drinkModificationService, IUserService userService)
        {
            this.drinkModificationService = drinkModificationService;
            this.userService = userService;
        }

        [HttpGet("get-all")]
        public async Task<IEnumerable<DrinkModificationRequest>> GetAll()
        {
            return await this.drinkModificationService.GetAllModificationRequests();
        }

        [HttpGet]
        public async Task<DrinkModificationRequest> Get(int modificationRequestId)
        {
            return await this.drinkModificationService.GetModificationRequest(modificationRequestId);
        }

        [HttpPost("deny")]
        public async Task Deny(int modificationRequestId, [FromBody] Guid userId)
        {
            var userRole = await userService.GetHighestRoleTypeForUser(userId);
            if (userRole != RoleType.Admin)
                return;

            await this.drinkModificationService.DenyRequest(modificationRequestId);
        }
    }
}
