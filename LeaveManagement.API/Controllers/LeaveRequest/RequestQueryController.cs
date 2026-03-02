using LeaveManagement.API.Constants;
using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveRequest;
using LeaveManagement.API.Infrastructure;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllApproveRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllPendingRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRejectedRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestByEmployee;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetApprovedRequestsByEmployee;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetPendingRequestsByEmployee;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetRejectedRequestsByEmployee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Controllers.LeaveRequest
{
    [Authorize]
    [EnableRateLimiting(RateLimit.PolicyName.PerUser)]
    [Route("LeaveManagement/LeaveRequest")]
    [ApiController]
    public class RequestQueryController(RequestQueryHandlers queryHander) : ControllerBase
    {
        [HttpGet("Approved")]
        public async Task<IActionResult> GetApprovedLeaveRequests([FromQuery] GetAllApproveRequestsQuery query)
        {
            ResultT<List<GetAllApproveRequestsDto>> result =
                await queryHander.GetAllApproved.Handle(query);

            return result.Match<List<GetAllApproveRequestsDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Pending")]
        public async Task<IActionResult> GetPendingLeaveRequests([FromQuery] GetAllPendingRequestsQuery query)
        {
            ResultT<List<GetAllPendingRequestsDto>> result =
                await queryHander.GetAllPending.Handle(query);

            return result.Match<List<GetAllPendingRequestsDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Rejected")]
        public async Task<IActionResult> GetRejectedLeaveRequests([FromQuery] GetAllRejectedRequestsQuery query)
        {
            ResultT<List<GetAllRejectedRequestsDto>> result =
                await queryHander.GetAllRejected.Handle(query);

            return result.Match<List<GetAllRejectedRequestsDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Employee/{employeeId:guid}/All")]
        public async Task<IActionResult> GetRequestsByEmployeeId([FromRoute] Guid employeeId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            ResultT<List<GetAllRequestByEmployeeDto>> result =
                await queryHander.GetEmployeeRequests.Handle(new GetAllRequestByEmployeeQuery(employeeId, pageSize, pageNumber));

            return result.Match<List<GetAllRequestByEmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Employee/{employeeId:guid}/Approved")]
        public async Task<IActionResult> GetApprovedRequestsByEmployeeId([FromRoute] Guid employeeId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            ResultT<List<GetApprovedRequestsByEmployeeDto>> result =
                await queryHander.GetEmployeeApprovedRequest.Handle(new GetApprovedRequestsByEmployeeQuery(employeeId, pageSize, pageNumber));

            return result.Match<List<GetApprovedRequestsByEmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Employee/{employeeId:guid}/Pending")]
        public async Task<IActionResult> GetPendingRequestsByEmployeeId([FromRoute] Guid employeeId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            ResultT<List<GetPendingRequestsByEmployeeDto>> result =
                await queryHander.GetPendingEmployeeRequest.Handle(new GetPendingRequestsByEmployeeQuery(employeeId, pageSize, pageNumber));

            return result.Match<List<GetPendingRequestsByEmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Employee/{employeeId:guid}/Rejected")]
        public async Task<IActionResult> GetRejectedRequestsByEmployeeId([FromRoute] Guid employeeId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            ResultT<List<GetRejectedRequestsByEmployeeDto>> result =
                await queryHander.GetEmployeeRejectedRequest.Handle(new GetRejectedRequestsByEmployeeQuery(employeeId, pageSize, pageNumber));

            return result.Match<List<GetRejectedRequestsByEmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }
    }
}