using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Shared;
using LeaveManagement.Domain.Value_Objects;

namespace LeaveManagement.Domain.Entities
{
    public class Employee : AggregateRoot
    {

        private readonly List<LeaveAllocation> _allocations = new();
        private readonly List<LeaveRequest> _requests = new();
        private Employee(Guid id, Name name, Email email, bool isActive, Department department) : base(id)
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

        public ResultT<LeaveAllocation> AllocateLeave(LeaveType leave)
        {
            if (leave is null)
                return DomainErrors.General.NullObject;

            if(_allocations.Any(a => a.LeaveTypeId == leave.Id))
                return DomainErrors.LeaveAllocation.DuplicateAllocation;

            var allocation = LeaveAllocation.Create(this, leave);

            if (allocation.isFailure)
                return DomainErrors.LeaveAllocation.InstanceCreationFailed;

            _allocations.Add(allocation.Value);

            return ResultT<LeaveAllocation>.Success(allocation.Value);
        }

        public ResultT<LeaveRequest> RequestLeave(DateTime startDate, DateTime endDate, string? description, LeaveType leaveType)
        {
            if (!IsActive)
                return DomainErrors.Employee.InactiveEmployeeRequest;

            if (_requests.Any(r => r.OverlapsWith(startDate, endDate)))
                return DomainErrors.LeaveRequest.OverLappingRequest;

            if (!_allocations.Any(a => a.LeaveTypeId == leaveType.Id))
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            var leaveRequest = LeaveRequest.Create(startDate, endDate, description, this, leaveType);

            if(leaveRequest.isFailure)
                return DomainErrors.LeaveRequest.InstanceCreationFailed;

            if (_requests.Contains(leaveRequest.Value))
                return DomainErrors.LeaveRequest.RequestAlreadyExists;

            _requests.Add(leaveRequest.Value);

            return ResultT<LeaveRequest>.Success(leaveRequest.Value);
        }

        public ResultT<LeaveAllocation> ApproveLeaveRequest(LeaveRequest request)
        {
            if (!_requests.Contains(request))
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            if (!IsActive)
                return DomainErrors.Employee.InactiveEmployeeRequest;

            if (request.Status != Enums.LeaveRequestStatus.Pending)
                return DomainErrors.LeaveRequest.InvalidRequestStatus;

            if (request.LeaveDays.Days <= 0)
                return DomainErrors.LeaveDays.InvalidLeaveDuration;

            var allocation = _allocations.FirstOrDefault(a => a.LeaveTypeId == request.LeaveTypeId);

            if (allocation is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            var deducted = allocation.Days.Deduct(request.LeaveDays.Days);

            if (deducted.isFailure)
                return DomainErrors.LeaveDays.InsufficientLeaveDays;

            var approved = request.Approve();

            if (approved.isFailure)
                return DomainErrors.LeaveRequest.InvalidRequestStatus;

            return ResultT<LeaveAllocation>.Success(allocation);
        }

        public ResultT<bool> AllocationExists(LeaveAllocation allocation)
        {
            if (allocation is null)
                return DomainErrors.General.NullObject;

            var alloc = _allocations.FirstOrDefault(a => a.LeaveTypeId == allocation.LeaveTypeId);

            if (alloc is null)
                return ResultT<bool>.Success(false);

            return ResultT<bool>.Success(true);
        }

        public ResultT<bool> LeaveRequestExists(LeaveRequest request)
        {
            if(request is null)
                return DomainErrors.General.NullObject;

            var req = _requests.FirstOrDefault(r => r.LeaveTypeId == request.LeaveTypeId);

            if (req is null)
                return ResultT<bool>.Success(false);
  
           return ResultT<bool>.Success(true);
        }
    }
}
