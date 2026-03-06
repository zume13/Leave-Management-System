using LeaveManagement.API.Constants;
using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveType;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Dto.Response.LeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeaves;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Controllers.LeaveType
{
    [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
    [Route("LeaveManagement/LeaveType")]
    [ApiController]
    public class LeaveTypeQueryController(TypeQueryHandlers handlers) : ControllerBase
    {
        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpGet("All")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllLeavesQuery query)
        {
            ResultT<List<LeavesDto>> result = await handlers.GetAllLeaves.Handle(query);

            return result.Match<List<LeavesDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpGet("{leaveId:guid}")]
        public async Task<IActionResult> GetById(Guid leaveId)
        {
            ResultT<LeavesDto> result = await handlers.GetLeaveById.Handle(new GetLeaveByIdQuery(leaveId));

            return result.Match<LeavesDto, IActionResult>(Ok, CustomResults.Problem);
        }
    }
}
