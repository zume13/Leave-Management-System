using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.LeaveType;
using LeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeaves;

namespace LeaveManagement.API.Handlers.LeaveType
{
    public class TypeQueryHandlers
    {
        public IQueryHandler<GetAllLeavesQuery, List<LeavesDto>> GetAllLeaves { get; }
        public TypeQueryHandlers(IQueryHandler<GetAllLeavesQuery, List<LeavesDto>> getAllLeaves) 
        { 
            this.GetAllLeaves = getAllLeaves;
        }
    }
}
