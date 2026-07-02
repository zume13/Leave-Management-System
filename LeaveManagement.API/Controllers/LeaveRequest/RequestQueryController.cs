using LeaveManagement.API.Constants;
using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveRequest;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllApproveRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllPendingRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRejectedRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestsByEmployee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Controllers.LeaveRequest
{
    [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
    [Route("leave-management/leave-request")]
    [ApiController]
    public class RequestQueryController(RequestQueryHandlers queryHandler) : ControllerBase
    {
        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpGet("all")]
        public async Task<IActionResult> GetApprovedLeaveRequests([FromQuery] GetAllRequestsQuery query)
        {
            ResultT<List<GetAllRequestsDto>> result =
                await queryHandler.GetAllRequests.Handle(query);

            return result.Match<List<GetAllRequestsDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedLeaveRequests([FromQuery] GetAllApproveRequestsQuery query)
        {
            ResultT<List<GetAllApproveRequestsDto>> result =
                await queryHandler.GetAllApproved.Handle(query);

            return result.Match<List<GetAllApproveRequestsDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingLeaveRequests([FromQuery] GetAllPendingRequestsQuery query)
        {
            ResultT<List<GetAllPendingRequestsDto>> result =
                await queryHandler.GetAllPending.Handle(query);

            return result.Match<List<GetAllPendingRequestsDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.ManagerAndAbove)]
        [HttpGet("rejected")]
        public async Task<IActionResult> GetRejectedLeaveRequests([FromQuery] GetAllRejectedRequestsQuery query)
        {
            ResultT<List<GetAllRejectedRequestsDto>> result =
                await queryHandler.GetAllRejected.Handle(query);

            return result.Match<List<GetAllRejectedRequestsDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [Authorize(Policy = Auth.Policies.EmployeeAndAbove)]
        [HttpGet("employee/{employeeId:guid}")]
        public async Task<IActionResult> GetApprovedRequestsByEmployeeId([FromRoute] Guid employeeId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            ResultT<List<GetAllRequestsByEmployeeDto>> result =
                await queryHandler.GetAllRequestsByEmployee.Handle(new GetAllRequestsByEmployeeQuery(employeeId, pageSize, pageNumber));

            return result.Match<List<GetAllRequestsByEmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }
    }
}