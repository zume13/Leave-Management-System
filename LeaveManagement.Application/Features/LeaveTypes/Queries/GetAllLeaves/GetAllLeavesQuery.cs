using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.LeaveType;

namespace LeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeaves
{
    public sealed record GetAllLeavesQuery : IQuery<List<LeavesDto>>;

}
