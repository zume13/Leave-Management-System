using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequests;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllApproveRequests
{
    public sealed record GetAllApproveRequestsQuery(int pageSize, int pageNumber) : IQuery<List<GetAllRequestsDto>>;
}
