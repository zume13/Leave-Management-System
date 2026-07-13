using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequests;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRejectedRequests
{
    public sealed record GetAllRejectedRequestsQuery(int pageSize, int pageNumber) : IQuery<List<GetAllRequestsDto>>;
}
