using Microsoft.AspNetCore.Mvc;
using WinUIApp.WebAPI.Services.DummyServices;
using WinUIApp.WebAPI.Requests;

namespace WinUIApp.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet("is-admin")]
        public IActionResult IsAdmin([FromQuery] int userId)
        {
            return Ok(adminService.IsAdmin(userId));
        }

        [HttpPost("send-notification")]
        public IActionResult SendNotification([FromBody] SendNotificationRequest request)
        {
            adminService.SendNotificationFromUserToAdmin(
                request.SenderUserId,
                request.UserModificationRequestType,
                request.UserModificationRequestDetails);
            return Ok();
        }
    }
} 
