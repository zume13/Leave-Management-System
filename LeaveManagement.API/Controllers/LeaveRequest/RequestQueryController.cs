using LeaveManagement.API.Extensions;
using LeaveManagement.API.Handlers.LeaveRequest;
using LeaveManagement.API.Infrastracture;
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
using static LeaveManagement.API.Constants.RateLimit;

namespace LeaveManagement.API.Controllers.LeaveRequest
{
    [Authorize]
    [EnableRateLimiting(RateLimits.PerUser)]
    [Route("LeaveManagement/LeaveRequest")]
    [ApiController]
    public class RequestQueryController(RequestQueryHandlers queryHander) : ControllerBase
    {
        [HttpGet("Approved")]
        public async Task<IActionResult> GetApprovedLeaveRequests()
        {
            ResultT<List<GetAllApproveRequestsDto>> result =
                await queryHander.GetAllApproved.Handle(new GetAllApproveRequestsQuery());

            return result.Match<List<GetAllApproveRequestsDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Pending")]
        public async Task<IActionResult> GetPendingLeaveRequests()
        {
            ResultT<List<GetAllPendingRequestsDto>> result =
                await queryHander.GetAllPending.Handle(new GetAllPendingRequestsQuery());

            return result.Match<List<GetAllPendingRequestsDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Rejected")]
        public async Task<IActionResult> GetRejectedLeaveRequests()
        {
            ResultT<List<GetAllRejectedRequestsDto>> result =
                await queryHander.GetAllRejected.Handle(new GetAllRejectedRequestsQuery());

            return result.Match<List<GetAllRejectedRequestsDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Employee/{id:guid}/All")]
        public async Task<IActionResult> GetRequestsByEmployeeId(Guid id)
        {
            ResultT<List<GetAllRequestByEmployeeDto>> result =
                await queryHander.GetEmployeeRequests.Handle(new GetAllRequestByEmployeeQuery(id));

            return result.Match<List<GetAllRequestByEmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Employee/{id:guid}/Approved")]
        public async Task<IActionResult> GetApprovedRequestsByEmployeeId(Guid id)
        {
            ResultT<List<GetApprovedRequestsByEmployeeDto>> result =
                await queryHander.GetEmployeeApprovedRequest.Handle(new GetApprovedRequestsByEmployeeQuery(id));

            return result.Match<List<GetApprovedRequestsByEmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Employee/{id:guid}/Pending")]
        public async Task<IActionResult> GetPendingRequestsByEmployeeId(Guid id)
        {
            ResultT<List<GetPendingRequestsByEmployeeDto>> result =
                await queryHander.GetPendingEmployeeRequest.Handle(new GetPendingRequestsByEmployeeQuery(id));

            return result.Match<List<GetPendingRequestsByEmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }

        [HttpGet("Employee/{id:guid}/Rejected")]
        public async Task<IActionResult> GetRejectedRequestsByEmployeeId(Guid id)
        {
            ResultT<List<GetRejectedRequestsByEmployeeDto>> result =
                await queryHander.GetEmployeeRejectedRequest.Handle(new GetRejectedRequestsByEmployeeQuery(id));

            return result.Match<List<GetRejectedRequestsByEmployeeDto>, IActionResult>(Ok, CustomResults.Problem);
        }
    }
}