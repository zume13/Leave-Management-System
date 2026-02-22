using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveRequest;
using LeaveManagement.API.Infrastracture;
using LeaveManagement.Application.Features.LeaveRequest.Commands.ApproveLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.RejectLeaveRequest;
using LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Controllers.LeaveRequest
{
    [Route("LeaveManagement/LeaveRequest")]
    [ApiController]
    public class RequestCommandController(RequestCommandHandlers commandHandlers) : ControllerBase
    {
        [HttpPost("Approve")]
        public async Task<IActionResult> Approve([FromBody] ApproveLeaveRequestCommand command)
        {
            Result result = await commandHandlers.Approve.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [HttpPost("Cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelLeaveRequestCommand command)
        {
            Result result = await commandHandlers.Cancel.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateLeaveRequestCommand command)
        {
            ResultT<Guid> result = await commandHandlers.Create.Handle(command);

            return result.Match<Guid, IActionResult>(id => Created(string.Empty, id), CustomResults.Problem);
        }

        [HttpPost("Reject")]
        public async Task<IActionResult> Reject([FromBody] RejectLeaveRequestCommand command)
        {
            Result result = await commandHandlers.Reject.Handle(command);
            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateLeaveRequestCommand command)
        {
            Result result = await commandHandlers.Update.Handle(command);

            return result.Match<IActionResult>(NoContent, CustomResults.Problem);
        }
    }
}