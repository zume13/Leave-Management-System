using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllApproveRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllPendingRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRejectedRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestsByEmployee;
using LeaveManagement.Application.Features.LeaveRequest.Queries;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequests;

namespace LeaveManagement.API.Handlers.LeaveRequest
{
    public class RequestQueryHandlers
    {
        public IQueryHandler<GetAllApproveRequestsQuery, List<GetAllRequestsDto>> GetAllApproved { get; }
        public IQueryHandler<GetAllPendingRequestsQuery, List<GetAllRequestsDto>> GetAllPending { get; }
        public IQueryHandler<GetAllRejectedRequestsQuery, List<GetAllRequestsDto>> GetAllRejected { get; }
        public IQueryHandler<GetAllRequestsByEmployeeQuery, List<GetAllRequestsByEmployeeDto>> GetAllRequestsByEmployee { get; }
        public IQueryHandler<GetAllRequestsQuery, List<GetAllRequestsDto>> GetAllRequests { get; }
        public RequestQueryHandlers(
            IQueryHandler<GetAllApproveRequestsQuery, List<GetAllRequestsDto>> _GetAllApproved,
            IQueryHandler<GetAllPendingRequestsQuery, List<GetAllRequestsDto>> _GetAllPending,
            IQueryHandler<GetAllRejectedRequestsQuery, List<GetAllRequestsDto>> _GetAllRejected,
            IQueryHandler<GetAllRequestsByEmployeeQuery, List<GetAllRequestsByEmployeeDto>> _GetEmployeeApprovedRequest,
            IQueryHandler<GetAllRequestsQuery, List<GetAllRequestsDto>> _GetAllRequests)
        {
            GetAllApproved = _GetAllApproved;
            GetAllPending = _GetAllPending;
            GetAllRejected = _GetAllRejected;
            GetAllRequestsByEmployee = _GetEmployeeApprovedRequest;
            GetAllRequests = _GetAllRequests;
        }
    }
}
