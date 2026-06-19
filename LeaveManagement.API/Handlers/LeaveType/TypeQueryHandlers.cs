using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.LeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeaves;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveById;

namespace LeaveManagement.API.Handlers.LeaveType
{
    public class TypeQueryHandlers
    {
        public IQueryHandler<GetAllLeavesQuery, List<LeavesDto>> GetAllLeaves { get; }
        public IQueryHandler<GetLeaveByIdQuery, LeavesDto> GetLeaveById { get; }
        public TypeQueryHandlers(IQueryHandler<GetAllLeavesQuery, List<LeavesDto>> getAllLeaves,
                                 IQueryHandler<GetLeaveByIdQuery, LeavesDto> getLeaveById) 
        { 
            this.GetAllLeaves = getAllLeaves;
            this.GetLeaveById = getLeaveById;
        }
    }
}
