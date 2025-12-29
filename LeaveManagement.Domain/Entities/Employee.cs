using LeaveManagement.Domain.Commons.Shared;
using LeaveManagement.Domain.Contracts;
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
        public void DeActivate() => IsActive = false;

        public static ResultT<Employee> Create(Name name, Email email, Department department )
        {
            if (department is null)
                return DomainErrors.Department.NulllDepartment;

            if (name is null)
                return DomainErrors.Employee.EmptyEmployeeName;

            if (email is null)
                return DomainErrors.Email.EmptyEmail;
            
            return ResultT<Employee>.Success(new Employee(Guid.NewGuid(), name, email, true, department));
        }
        public Result AllocateLeave(LeaveType leave)
        {

            if(_allocations.Any(a => a.LeaveTypeId == leave.Id))
                return DomainErrors.LeaveAllocation.DuplicateAllocation;

            var allocation = LeaveAllocation.Create(this, leave);

            if (allocation.isFailure)
                return allocation.Error;

            _allocations.Add(allocation.Value);

            return Result.Success();
        }
        public ResultT<LeaveDuration> RemainingAllocatedLeaveDays(Guid allocationId)
        {
            var allocation = _allocations.SingleOrDefault(a => a.Id == allocationId);

            if (allocation is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            return ResultT<LeaveDuration>.Success(allocation.Days);
        }
        public Result RequestLeave(DateTime startDate, DateTime endDate, string? description, LeaveType leaveType )
        {
            if (!IsActive)
                return DomainErrors.Employee.InactiveEmployeeRequest;

            if (leaveType is null)
                return DomainErrors.LeaveType.NullLeaveType;

            if (startDate > endDate)
                return DomainErrors.LeaveRequest.InvalidDateRange;

            if (_requests.Any(r => r.OverlapsWith(startDate, endDate)))
                return DomainErrors.LeaveRequest.OverLappingRequest;

            var alloc = _allocations.SingleOrDefault(a => a.LeaveTypeId == leaveType.Id);
                
            if(alloc is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            if (_requests.Any(r =>
                r.LeaveTypeId == leaveType.Id &&
                r.StartDate == startDate &&
                r.EndDate == endDate && 
                r.IsPending()))
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
        public Result UpdateLeaveRequest(Guid requestId, Action<LeaveRequest> method)
        {
            var request = _requests.SingleOrDefault(r => r.Id == requestId);

            if (request is null)
                return DomainErrors.LeaveRequest.RequestNotFound;

            if (!request.IsPending())
                return DomainErrors.LeaveRequest.InvalidRequestStatus;

            method(request);

            return Result.Success();
        }
        public Result CancelLeaveRequest(Guid requestId) => UpdateLeaveRequest(requestId, r => r.Cancel());
        public Result RejectLeaveRequest(Guid requestId) => UpdateLeaveRequest(requestId, r => r.Reject()); 
        public Result ApproveLeaveRequest(Guid requestId)
        {

            var request = _requests.SingleOrDefault(r => r.Id == requestId);

            if(request is null)
                return DomainErrors.LeaveRequest.RequestNotFound;

            if (!IsActive)
                return DomainErrors.Employee.InactiveEmployeeRequest;

            var allocation = _allocations.SingleOrDefault(a => a.LeaveTypeId == request.LeaveTypeId);

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

            allocation.UpdateDays(deducted.Value);

            return Result.Success();
        }
        public ResultT<LeaveAllocation> HasAllocationForLeaveType(Guid leaveTypeId)
        {
            var alloc = _allocations.SingleOrDefault(a => a.LeaveTypeId == leaveTypeId);

            if (alloc is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            return ResultT<LeaveAllocation>.Success(alloc);
        }
        public ResultT<LeaveRequest> HasPendingRequest(Guid leaveTypeId)
        {
            var request = _requests.SingleOrDefault(r => r.LeaveTypeId == leaveTypeId && r.IsPending());

            if (request is null)
                return DomainErrors.LeaveRequest.RequestNotFound;

            return ResultT<LeaveRequest>.Success(request);
        }
        public List<LeaveRequest> GetAllPendingRequests()
        {
            return _requests.Where(r => r.IsPending()).ToList();
        }
        public Result ChangeEmail(string email)
        {
           var result = Email.Create(email);

            if (result.isFailure)
                return result.Error;

            Email = result.Value;
            return Result.Success();
        }
        public Result ChangeName(string name)
        {
            var result = Name.Create(name);

            if (result.isFailure) 
                return result.Error;

            Name = result.Value;
            return Result.Success();
        }
        
        //raise domain events 
        //add attribute for who processed leaves and allocation
    }
}
