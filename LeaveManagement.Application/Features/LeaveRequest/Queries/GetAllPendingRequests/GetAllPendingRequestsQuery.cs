using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllPendingRequests
{
    public sealed record GetAllPendingRequestsQuery(int pageSize, int pageNumber) : IQuery<List<GetAllPendingRequestsDto>>;
}
