using LeaveManagement.API.Constants;
using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveRequest;
using LeaveManagement.API.Infrastracture;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.RejectLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Controllers.LeaveRequest
{
    [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
    [Route("leave-management/leave-request")]
    [ApiController]
    public class RequestCommandController(RequestCommandHandlers commandHandlers) : ControllerBase
    {
        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpPost("approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveLeaveRequestCommand command)
        {
            Result result = await commandHandlers.Approve.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.EmployeeAndAbove)]
        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelLeaveRequestCommand command)
        {
            Result result = await commandHandlers.Cancel.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.EmployeeAndAbove)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateLeaveRequestCommand command)
        {
            ResultT<Guid> result = await commandHandlers.Create.Handle(command);

            return result.Match<Guid, IActionResult>(id => Created(string.Empty, id), CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpPost("reject")]
        public async Task<IActionResult> Reject([FromBody] RejectLeaveRequestCommand command)
        {
            Result result = await commandHandlers.Reject.Handle(command);
            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.EmployeeAndAbove)]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateLeaveRequestCommand command)
        {
            Result result = await commandHandlers.Update.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }
    }
}