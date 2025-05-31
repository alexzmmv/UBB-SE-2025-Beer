using DataAccess.AutoChecker;
using DataAccess.Constants;
using DataAccess.Data;
using DataAccess.Service;
using DataAccess.DTOModels;
using DataAccess.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Requests.Drinks;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/DrinkModificationRequests")]
    public class DrinkModificationRequestsController: ControllerBase
    {
        private readonly IDrinkModificationRequestService drinkModificationService;
        private readonly IUserService userService;

        public DrinkModificationRequestsController(IDrinkModificationRequestService drinkModificationService, IUserService userService)
        {
            this.drinkModificationService = drinkModificationService;
            this.userService = userService;
        }

        [HttpGet("get-all")]
        public async Task<IEnumerable<DrinkModificationRequestDTO>> GetAll()
        {
            return await this.drinkModificationService.GetAllModificationRequests();
        }

        [HttpGet]
        public async Task<DrinkModificationRequestDTO> Get(int modificationRequestId)
        {
            return await this.drinkModificationService.GetModificationRequest(modificationRequestId);
        }

        [HttpPost("deny")]
        public async Task<IActionResult> Deny([FromBody] DenyDrinkModificationRequest request)
        {
            var userRole = await userService.GetHighestRoleTypeForUser(request.userId);
            if (userRole != RoleType.Admin)
                return Unauthorized();
            
            await this.drinkModificationService.DenyRequest(request.ModificationRequestId, new Guid());
            return Ok();
        }

        [HttpPost("approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveDrinkModificationRequest request)
        {
            var userRole = await userService.GetHighestRoleTypeForUser(request.userId);
            if (userRole != RoleType.Admin)
                return Unauthorized();

            await this.drinkModificationService.ApproveRequest(request.ModificationRequestId, new Guid());
            return Ok();
        }

        [HttpPost("add")]
        public async Task<DrinkModificationRequestDTO> Add([FromBody] AddDrinkModificationRequestRequest request)
        {
            return this.drinkModificationService.AddRequest(
                request.ModificationType,
                request.OldDrinkId,
                request.NewDrinkId,
                request.RequestingUserId);
        }
    }
}
