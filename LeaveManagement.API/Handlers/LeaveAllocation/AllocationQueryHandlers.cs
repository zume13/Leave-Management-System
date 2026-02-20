using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Allocation;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetActiveLeaveAllocations;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllAllocationsByEmployee;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetAllocationByEmployee;
using LeaveManagement.Application.Features.LeaveAllocation.Queries.GetExpiredLeaveAllocations;

namespace LeaveManagement.API.Handlers.LeaveAllocation
{
    public class AllocationQueryHandlers
    {
        public IQueryHandler<GetActiveLeaveAllocationsQuery, List<LeaveAllocationDto>> GetActive { get; }
        public IQueryHandler<GetAllAllocationsByEmployeeQuery, List<GetAllocationByEmployeeDto>> GetEmployeeAllocations { get; }
        public IQueryHandler<GetExpiredLeaveAllocationsQuery, List<LeaveAllocationDto>> GetExpired { get; }
        public AllocationQueryHandlers(
            IQueryHandler<GetActiveLeaveAllocationsQuery, List<LeaveAllocationDto>> _GetActive,
            IQueryHandler<GetAllAllocationsByEmployeeQuery, List<GetAllocationByEmployeeDto>> _GetEmployeeAllocations,
            IQueryHandler<GetExpiredLeaveAllocationsQuery, List<LeaveAllocationDto>> _GetExpired)
        {
            GetActive = _GetActive;
            GetEmployeeAllocations = _GetEmployeeAllocations;
            GetExpired = _GetExpired;
        }
    }
}
