using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Queries.GetAllRequests
{
    public sealed record GetAllRequestsQuery(
         int pageNumber,
         int pageSize
     ) : IQuery<List<GetAllRequestsDto>>;
}
