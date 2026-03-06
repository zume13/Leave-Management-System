using LeaveManagement.API.Constants;
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

namespace LeaveManagement.API.Controllers.LeaveAllocation
{
    [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
    [Route("LeaveManagement/Allocation")]
    [ApiController]
    public class AllocationCommandController(AllocationCommandHandlers commandHandlers) : ControllerBase
    {
        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpPost("Allocate")]
        public async Task<IActionResult> Allocate([FromBody] AllocateLeaveCommand command)
        {
            ResultT<Guid> result = await commandHandlers.Allocate.Handle(command);

            return result.Match<Guid, IActionResult>(id => Created(string.Empty, id), CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpPost("Bulk")]
        public async Task<IActionResult> BulkAllocate([FromBody] BulkLeaveAllocationCommand command)
        {
            ResultT<BulkLeaveAllocationDto> result = await commandHandlers.BulkAllocate.Handle(command);

            return result.Match<BulkLeaveAllocationDto, IActionResult>(Ok, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpDelete("Delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result result = await commandHandlers.Delete.Handle(new DeleteLeaveAllocationCommand(id));

            return result.Match(Ok, CustomResults.Problem);
        }
    }
}

