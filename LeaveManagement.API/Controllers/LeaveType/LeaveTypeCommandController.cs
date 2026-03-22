using LeaveManagement.API.Constants;
using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveType;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeave;
using LeaveManagement.Application.Features.LeaveTypes.Commands.RemoveLeave;
using LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Controllers.LeaveType
{
    [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
    [Route("leave-management/leave-type")]
    [ApiController]
    public class LeaveTypeCommandController(TypeCommandHandlers handlers) : ControllerBase
    {
        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateLeaveCommand command)
        {
            ResultT<Guid> result = await handlers.Create.Handle(command);

            return result.Match<Guid, IActionResult>(id => Created(string.Empty, id), CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result result = await handlers.Remove.Handle(new RemoveLeaveTypeCommand(id));

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateLeaveTypeCommand command)
        {
            Result result = await handlers.Update.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }
    }
}