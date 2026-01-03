using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Primitives;
using SharedKernel.Shared;
using LeaveManagement.Domain.Value_Objects;


namespace LeaveManagement.Domain.Entities
{
    public class LeaveAllocation : Entity
    {
        private LeaveAllocation(Guid id, Employee employee, LeaveType leaveType, int year, DateTime creationDate) : base(id)
        {
            CreationDate = creationDate;
            LeaveDays = leaveType.Days;
            Year = year;
            EmployeeId = employee.Id;
            LeaveTypeId = leaveType.Id;
        }
        private LeaveAllocation() { }
        public DateTime CreationDate { get; private set; }
        public LeaveDuration LeaveDays { get; private set; }
        public int UsedDays { get; private set; } = default;
        public int RemainingDays
        {
            get
            {
                return LeaveDays.Days - UsedDays;
            }
        }

        public int Year { get; private set; }

        //FKs
        public Guid EmployeeId { get; private set; }
        public Guid LeaveTypeId { get; private set; }

        public static ResultT<LeaveAllocation> Create(Employee employee, LeaveType leaveType)
        {
            if (employee is null)
                return DomainErrors.Employee.NullEmployee;

            if (employee.Status == EmployeeStatus.Fired)
                return DomainErrors.Employee.InactiveEmployee;

            if (leaveType is null)
                return DomainErrors.LeaveType.NullLeaveType;

            return ResultT<LeaveAllocation>.Success(new LeaveAllocation(Guid.NewGuid(), employee, leaveType, DateTime.UtcNow.Year, DateTime.UtcNow));
        } 

        public void Consume(int days)
        {
            UsedDays += days;
        }
        
        public bool CanConsume(int days) => RemainingDays >= days;

        //raise updated allocation days
    }
}
