using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRejectedRequests
{
    public sealed record GetAllRejectedRequestsQuery : IQuery<List<GetAllRejectedRequestsDto>>;
}
