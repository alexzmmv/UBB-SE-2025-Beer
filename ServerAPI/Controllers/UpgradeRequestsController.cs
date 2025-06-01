﻿using Microsoft.AspNetCore.Mvc;
using DataAccess.Model.AdminDashboard;
using DataAccess.Service.Interfaces;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/upgradeRequests")]
    public class UpgradeRequestsController : ControllerBase
    {
        private readonly IUpgradeRequestsService upgradeRequestsService;

        public UpgradeRequestsController(IUpgradeRequestsService upgradeRequestsService)
        {
            this.upgradeRequestsService = upgradeRequestsService;
        }

        [HttpGet]
        public async Task<IEnumerable<UpgradeRequest>> GetAll()
        {
            return await this.upgradeRequestsService.RetrieveAllUpgradeRequests();
        }

        [HttpPost("add")]
        public async Task Add([FromBody] Guid userId)
        {
            await this.upgradeRequestsService.AddUpgradeRequest(userId);
        }

        [HttpDelete("{id}/delete")]
        public async Task Delete(int id)
        {
            await this.upgradeRequestsService.RemoveUpgradeRequestByIdentifier(id);
        }

        [HttpGet("{id}")]
        public async Task<UpgradeRequest?> Get(int id)
        {
            return await this.upgradeRequestsService.RetrieveUpgradeRequestByIdentifier(id);
        }

        [HttpPost("{id}/process")]
        public async Task Process(int id, [FromBody] bool isAccepted)
        {
            await this.upgradeRequestsService.ProcessUpgradeRequest(isAccepted, id);
        }

       [HttpGet("user/{userId:guid}/hasPending")]
        public async Task<bool> HasPending(Guid userId)
        {
            return await this.upgradeRequestsService.HasPendingUpgradeRequest(userId);
        }
    }
}
