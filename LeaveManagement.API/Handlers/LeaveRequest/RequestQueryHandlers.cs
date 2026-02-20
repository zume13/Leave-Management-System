using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllApproveRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllPendingRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRejectedRequests;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequestByEmployee;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetApprovedRequestsByEmployee;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetPendingRequestsByEmployee;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetRejectedRequestsByEmployee;

namespace LeaveManagement.API.Handlers.LeaveRequest
{
    public class RequestQueryHandlers
    {
        public IQueryHandler<GetAllApproveRequestsQuery, List<GetAllApproveRequestsDto>> GetAllApproved { get; }
        public IQueryHandler<GetAllPendingRequestsQuery, List<GetAllPendingRequestsDto>> GetAllPending { get; }
        public IQueryHandler<GetAllRejectedRequestsQuery, List<GetAllRejectedRequestsDto>> GetAllRejected { get; }
        public IQueryHandler<GetAllRequestByEmployeeQuery, List<GetAllRequestByEmployeeDto>> GetEmployeeRequests { get; }
        public IQueryHandler<GetApprovedRequestsByEmployeeQuery, List<GetApprovedRequestsByEmployeeDto>> GetEmployeeApprovedRequest { get; }
        public IQueryHandler<GetPendingRequestsByEmployeeQuery, List<GetPendingRequestsByEmployeeDto>> GetPendingEmployeeRequest { get; }
        public IQueryHandler<GetRejectedRequestsByEmployeeQuery, List<GetRejectedRequestsByEmployeeDto>> GetEmployeeRejectedRequest { get; }
        public RequestQueryHandlers(
            IQueryHandler<GetAllApproveRequestsQuery, List<GetAllApproveRequestsDto>> _GetAllApproved,
            IQueryHandler<GetAllPendingRequestsQuery, List<GetAllPendingRequestsDto>> _GetAllPending,
            IQueryHandler<GetAllRejectedRequestsQuery, List<GetAllRejectedRequestsDto>> _GetAllRejected,
            IQueryHandler<GetAllRequestByEmployeeQuery, List<GetAllRequestByEmployeeDto>> _GetEmployeeRequests,
            IQueryHandler<GetApprovedRequestsByEmployeeQuery, List<GetApprovedRequestsByEmployeeDto>> _GetEmployeeApprovedRequest,
            IQueryHandler<GetPendingRequestsByEmployeeQuery, List<GetPendingRequestsByEmployeeDto>> _GetPendingEmployeeRequest,
            IQueryHandler<GetRejectedRequestsByEmployeeQuery, List<GetRejectedRequestsByEmployeeDto>> _GetEmployeeRejectedRequest)
        {
            GetAllApproved = _GetAllApproved;
            GetAllPending = _GetAllPending;
            GetAllRejected = _GetAllRejected;
            GetEmployeeRequests = _GetEmployeeRequests;
            GetEmployeeApprovedRequest = _GetEmployeeApprovedRequest;
            GetPendingEmployeeRequest = _GetPendingEmployeeRequest;
            GetEmployeeRejectedRequest = _GetEmployeeRejectedRequest;
        }
    }
}
