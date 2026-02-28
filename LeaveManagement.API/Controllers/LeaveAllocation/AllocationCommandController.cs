using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveAllocation;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Dto.Response.LeaveAllocation;
using LeaveManagement.Application.Features.LeaveAllocation.Commands.AllocateLeave;
using LeaveManagement.Application.Features.LeaveAllocation.Commands.BulkAllocateLeave;
using LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;
using static LeaveManagement.API.Constants.RateLimit;

namespace LeaveManagement.API.Controllers.LeaveAllocation
{
    [Authorize]
    [Route("LeaveManagement/Allocation")]
    [EnableRateLimiting(RateLimits.PerUser)]
    [ApiController]
    public class AllocationCommandController(AllocationCommandHandlers commandHandlers) : ControllerBase
    {
        [HttpPost("Allocate")]
        public async Task<IActionResult> Allocate([FromBody] AllocateLeaveCommand command)
        {
            ResultT<Guid> result = await commandHandlers.Allocate.Handle(command);

            return result.Match<Guid, IActionResult>(id => Created(string.Empty, id), CustomResults.Problem);
        }

        [HttpPost("Bulk")]
        public async Task<IActionResult> BulkAllocate([FromBody] BulkLeaveAllocationCommand command)
        {
            ResultT<BulkLeaveAllocationDto> result = await commandHandlers.BulkAllocate.Handle(command);

            return result.Match<BulkLeaveAllocationDto, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpDelete("Delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result result = await commandHandlers.Delete.Handle(new DeleteLeaveAllocationCommand(id));

            return result.Match(Ok, CustomResults.Problem);
        }
    }
}

