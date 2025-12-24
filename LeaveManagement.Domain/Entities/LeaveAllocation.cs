

using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Shared;
using LeaveManagement.Domain.Value_Objects;


namespace LeaveManagement.Domain.Entities
{
    public class LeaveAllocation : Entity
    {
        internal LeaveAllocation(Guid id, Employee employee, LeaveType leaveType) : base(id)
        {
            CreationDate = DateTime.UtcNow;
            Days = leaveType.Days;
            Year = DateTime.UtcNow.Year;
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
        
        // Nav Properties
        public Employee? Employee { get; private set; }
        public LeaveType? LeaveType { get; private set; }

        public static ResultT<LeaveAllocation> Create(Employee employee, LeaveType leaveType)
        {
            if (employee is null)
                return DomainErrors.General.NullObject;

            if (leaveType is null)
                return DomainErrors.General.NullObject;

            return ResultT<LeaveAllocation>.Success(new LeaveAllocation(Guid.NewGuid(), employee, leaveType));
        } 

    }
}
