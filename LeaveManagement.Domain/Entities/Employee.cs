using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Shared;
using LeaveManagement.Domain.Value_Objects;

namespace LeaveManagement.Domain.Entities
{
    public class Employee : AggregateRoot
    {

        private readonly List<LeaveAllocation> _allocations = new();
        private readonly List<LeaveRequest> _requests = new();
        private Employee(
            Guid id, 
            Name name, 
            Email email, 
            bool isActive, 
            Department department) : base(id)
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

        public IReadOnlyCollection<LeaveAllocation> Allocations => _allocations;
        public IReadOnlyCollection<LeaveRequest> Requests => _requests;

        public void Activate() => IsActive = true;
        public void DeActivate() => IsActive = false; //give invariants

        public static ResultT<Employee> Create(
            Name name, 
            Email email, 
            Department department )
        {
            if (department is null)
                return DomainErrors.General.NullObject;

            if (name is null)
                return DomainErrors.Employee.EmptyEmployeeName;

            if (email is null)
                return DomainErrors.Email.EmptyEmail;

            return ResultT<Employee>.Success(new Employee(Guid.NewGuid(), name, email, true, department));
        }

        public Result AllocateLeave(LeaveType leave)
        {
            if (leave is null)
                return DomainErrors.General.NullObject;

            if(_allocations.Any(a => a.LeaveTypeId == leave.Id))
                return DomainErrors.LeaveAllocation.DuplicateAllocation;

            var allocation = LeaveAllocation.Create(this, leave);

            if (allocation.isFailure)
                return allocation.Error;

            _allocations.Add(allocation.Value);

            return Result.Success();
        }

        public Result RequestLeave
            (DateTime startDate, 
             DateTime endDate, 
             string? description, 
             LeaveType leaveType )
        {
            if (!IsActive)
                return DomainErrors.Employee.InactiveEmployeeRequest;

            if (leaveType is null)
                return DomainErrors.General.NullObject;

            if (startDate > endDate)
                return DomainErrors.LeaveRequest.InvalidDateRange;

            if (_requests.Any(r => r.OverlapsWith(startDate, endDate)))
                return DomainErrors.LeaveRequest.OverLappingRequest;

            var alloc = _allocations.FirstOrDefault(a => a.LeaveTypeId == leaveType.Id);
                
            if(alloc is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            if (_requests.Any(r =>
                r.LeaveTypeId == leaveType.Id &&
                r.StartDate == startDate &&
                r.EndDate == endDate))
                return DomainErrors.LeaveRequest.RequestAlreadyExists;

            if (alloc.Days.Days < LeaveRequest.GetLeaveSpan(startDate, endDate)) 
                return DomainErrors.LeaveDays.InsufficientLeaveDays;

            var leaveRequest = LeaveRequest.Create(
                startDate, 
                endDate, 
                description, 
                this, 
                leaveType );

            if(leaveRequest.isFailure)
                return leaveRequest.Error;

            _requests.Add(leaveRequest.Value);

            return Result.Success();
        }

        public Result ApproveLeaveRequest(LeaveRequest request)
        {
            if (request is null)
                return DomainErrors.General.NullObject;

            if (request.EmployeeId != this.Id)
                return DomainErrors.LeaveRequest.InvalidEmployee;

            if (!IsActive)
                return DomainErrors.Employee.InactiveEmployeeRequest;

            var allocation = _allocations.FirstOrDefault(a => a.LeaveTypeId == request.LeaveTypeId);

            if (allocation is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            if (!request.IsPending())
                return DomainErrors.LeaveRequest.InvalidRequestStatus;

            if (allocation.Days.Days < request.LeaveDays.Days)
                return DomainErrors.LeaveDays.InsufficientLeaveDays;

            var approved = request.Approve();

            if (approved.isFailure)
                return approved.Error;

            var deducted = allocation.Days.Deduct(request.LeaveDays.Days);

            if (deducted.isFailure)
                return deducted.Error;

            return Result.Success();
        }

        public ResultT<LeaveAllocation> HasAllocationFor(LeaveAllocation allocation)
        {
            if (allocation is null)
                return DomainErrors.General.NullObject;

            var alloc = _allocations.FirstOrDefault(a => a.LeaveTypeId == allocation.LeaveTypeId);

            if (alloc is null)
                return DomainErrors.General.NullObject;

            return ResultT<LeaveAllocation>.Success(alloc);
        }

        public ResultT<LeaveRequest> HasPendingRequest(LeaveRequest request)
        {
            if(request is null)
                return DomainErrors.General.NullObject;

            var req = _requests.FirstOrDefault(r => r.LeaveTypeId == request.LeaveTypeId);

            if (req is null)
                return DomainErrors.General.NullObject;

            return ResultT<LeaveRequest>.Success(req);
        }

        //raise domain events for approved and rejected leaves
        //cancel request
        //reject leave
        //allocate in bulk
        //get remaining leavedays
        //list pending request
        //email and name update
        //add attributes for who created and processed leaves and allocation
        //add invariant that employee cannot approve his own leave
    }
}
