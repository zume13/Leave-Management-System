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
        public IQueryHandler<GetAllApproveRequestsQuery, List<GetAllApproveRequestsDto>> GetAllApproved { get; }
        public IQueryHandler<GetAllPendingRequestsQuery, List<GetAllPendingRequestsDto>> GetAllPending { get; }
        public IQueryHandler<GetAllRejectedRequestsQuery, List<GetAllRejectedRequestsDto>> GetAllRejected { get; }
        public IQueryHandler<GetAllRequestsByEmployeeQuery, List<GetAllRequestsByEmployeeDto>> GetAllRequestsByEmployee { get; }
        public IQueryHandler<GetAllRequestsQuery, List<GetAllRequestsDto>> GetAllRequests { get; }
        public RequestQueryHandlers(
            IQueryHandler<GetAllApproveRequestsQuery, List<GetAllApproveRequestsDto>> _GetAllApproved,
            IQueryHandler<GetAllPendingRequestsQuery, List<GetAllPendingRequestsDto>> _GetAllPending,
            IQueryHandler<GetAllRejectedRequestsQuery, List<GetAllRejectedRequestsDto>> _GetAllRejected,
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
