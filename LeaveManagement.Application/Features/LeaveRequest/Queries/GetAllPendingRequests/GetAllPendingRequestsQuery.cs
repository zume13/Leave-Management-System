using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequests;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllPendingRequests
{
    public sealed record GetAllPendingRequestsQuery(int pageSize, int pageNumber) : IQuery<List<GetAllRequestsDto>>;
}
