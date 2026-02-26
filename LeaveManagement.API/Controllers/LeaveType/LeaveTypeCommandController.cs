using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveType;
using LeaveManagement.API.Infrastracture;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeave;
using LeaveManagement.Application.Features.LeaveTypes.Commands.RemoveLeave;
using LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;
using static LeaveManagement.API.Constants.Constants;

namespace LeaveManagement.API.Controllers.LeaveType
{
    [Authorize]
    [EnableRateLimiting(RateLimits.PerUser)]
    [Route("LeaveManagement/LeaveType")]
    [ApiController]
    public class LeaveTypeCommandController(TypeCommandHandlers handlers) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateLeaveCommand command)
        {
            ResultT<Guid> result = await handlers.Create.Handle(command);

            return result.Match<Guid, IActionResult>(id => Created(string.Empty, id), CustomResults.Problem);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result result = await handlers.Remove.Handle(new RemoveLeaveTypeCommand(id));

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateLeaveTypeCommand command)
        {
            Result result = await handlers.Update.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }
    }
}