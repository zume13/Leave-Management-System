using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Shared;
using LeaveManagement.Domain.Value_Objects;

namespace LeaveManagement.Domain.Entities
{
    public class Employee : AggregateRoot
    {

        private readonly List<LeaveAllocation> _allocations = new();
        private readonly List<LeaveRequest> _requests = new();
        private Employee(Guid id, Name name, Email email, bool isActive,  Department department) : base(id)
        {
            this.Name = name;
            this.Email = email;
            this.IsActive = isActive;
            this.DeptId = department.Id;
        }
        private Employee() { }

        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public bool IsActive { get; private set; }

        //FKs
        public Guid DeptId { get; private set; }

        //Nav Property
        public Department? Department { get; private set; }

        public IReadOnlyCollection<LeaveAllocation> allocations => _allocations;
        public IReadOnlyCollection<LeaveRequest> requests => _requests;

        public void Activate() => IsActive = true;
        public void DeActivate() => IsActive = false;

        public static ResultT<Employee> Create(Name name, Email email, Department department)
        {
            if (name is null)
                    return DomainErrors.Employee.EmptyEmployeeName;

            if (email is null)
                return DomainErrors.Email.EmptyEmail;

            return ResultT<Employee>.Success(new Employee(Guid.NewGuid(), name, email, true, department));

        }

        public ResultT<LeaveAllocation> GrantLeave(LeaveType leave)
        {
            if (leave is null)
                return DomainErrors.General.NullObject;

            var allocation = LeaveAllocation.Create(this, leave);

            if (allocation.isFailure)
                return DomainErrors.LeaveAllocation.InstanceCreationFailed;

            _allocations.Add(allocation.Value);

            return ResultT<LeaveAllocation>.Success(allocation.Value);

        }

        public ResultT<LeaveAllocation> UseLeave(LeaveType leave, int Days)
        {
            if (Days <= 0)
                return DomainErrors.General.InvalidInt;

            if(leave is null)
                return DomainErrors.General.NullObject;

            var allocation = _allocations.FirstOrDefault(l => l.LeaveType.Id == leave.Id && l.Year == leave.Period);

            if(allocation is null)
                return DomainErrors.General.NotFound;

            var deductResult = allocation.Days.Deduct(Days);

            if(deductResult.isFailure)
                return DomainErrors.LeaveDays.FailedDeduction;

            return ResultT<LeaveAllocation>.Success(allocation);
        }

    }
}
