using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllApproveRequests
{
    public sealed record GetAllApproveRequestsQuery : IQuery<List<GetAllApproveRequestsDto>>;
}
