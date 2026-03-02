using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.LeaveType;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeaves
{
    public sealed record GetAllLeavesQuery(int pageSize, int pageNumber) : IQuery<List<LeavesDto>>;

}
