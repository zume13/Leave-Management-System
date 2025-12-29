using LeaveManagement.Domain.Commons.Shared;
using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Shared;
using LeaveManagement.Domain.Value_Objects;


namespace LeaveManagement.Domain.Entities
{
    public class LeaveAllocation : Entity
    {
        private LeaveAllocation(Guid id, Employee employee, LeaveType leaveType, int year, DateTime creationDate) : base(id)
        {
            CreationDate = creationDate;
            Days = leaveType.Days;
            Year = year;
            EmployeeId = employee.Id;
            LeaveTypeId = leaveType.Id;
        }
        private LeaveAllocation() { }
        public DateTime CreationDate { get; private set; }
        public LeaveDuration Days { get; private set; }
        public int Year { get; private set; }

        //FKs
        public Guid EmployeeId { get; private set; }
        public Guid LeaveTypeId { get; private set; }

        public static ResultT<LeaveAllocation> Create(Employee employee, LeaveType leaveType)
        {
            if (employee is null)
                return DomainErrors.Employee.NullEmployee;

            if (leaveType is null)
                return DomainErrors.LeaveType.NullLeaveType;

            return ResultT<LeaveAllocation>.Success(new LeaveAllocation(Guid.NewGuid(), employee, leaveType, DateTime.UtcNow.Year, DateTime.UtcNow));
        } 

        public Result UpdateDays(LeaveDuration newDays)
        {
            if (newDays is null)
                return DomainErrors.LeaveDays.NullLeaveDays;

            if (newDays == Days)
                return Result.Success();

            Days = newDays;

            return Result.Success();
        }
    }
}
