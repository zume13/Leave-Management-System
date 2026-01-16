using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllPendingRequests
{
    public sealed record GetAllPendingRequestsQuery : IQuery<List<GetAllPendingRequestsDto>>;
}
