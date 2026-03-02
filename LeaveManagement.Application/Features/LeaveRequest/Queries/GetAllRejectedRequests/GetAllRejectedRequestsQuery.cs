using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRejectedRequests
{
    public sealed record GetAllRejectedRequestsQuery(int pageSize, int pageNumber) : IQuery<List<GetAllRejectedRequestsDto>>;
}
