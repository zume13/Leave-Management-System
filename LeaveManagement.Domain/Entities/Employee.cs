using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Primitives;
using SharedKernel.Shared;
using LeaveManagement.Domain.Value_Objects;

namespace LeaveManagement.Domain.Entities
{
    public class Employee : AggregateRoot
    {
        private readonly List<LeaveAllocation> _allocations = new();
        private readonly List<LeaveRequest> _requests = new();
        private Employee(Guid id, Name name, Email email, EmployeeStatus status, Guid departmentId, string userId) : base(id)
        {
            this.Name = name;
            this.Email = email;
            this.Status = status;
            this.DeptId = departmentId;
            this.UserId = userId;
        }
        private Employee() { }

        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public EmployeeStatus Status { get; private set; }

        //FKs
        public Guid DeptId { get; private set; }
        public string UserId { get; private set; } = null!;

        public IReadOnlyCollection<LeaveAllocation> Allocations => _allocations;
        public IReadOnlyCollection<LeaveRequest> Requests => _requests;

        #region methods
        public static ResultT<Employee> Create(Name name, Email email, Guid departmentId, string userId)
        {
            if (userId is null)
                return DomainErrors.Employee.NullUserId;

            if (name is null)
                return DomainErrors.Employee.EmptyEmployeeName;

            if (email is null)
                return DomainErrors.Email.EmptyEmail;
            
            return ResultT<Employee>.Success(new Employee(Guid.NewGuid(), name, email, EmployeeStatus.Active, departmentId, userId));
        }
        public ResultT<Guid> AllocateLeave(LeaveType leave)
        {
            if (Status == EmployeeStatus.Fired)
                return DomainErrors.Employee.InactiveEmployee;

            if(_allocations.Any(a => a.LeaveTypeId == leave.Id))
                return DomainErrors.LeaveAllocation.DuplicateAllocation;

            var allocation = LeaveAllocation.Create(this, leave);

            if (allocation.isFailure)
                return allocation.Error;

            _allocations.Add(allocation.Value);

            return ResultT<Guid>.Success(allocation.Value.Id);
        }
        public ResultT<LeaveDuration> GetRemainingLeaveDays(Guid allocationId)
        {
            var allocation = _allocations.FirstOrDefault(a => a.Id == allocationId);

            if (allocation is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            return ResultT<LeaveDuration>.Success(allocation.LeaveDays);
        }
        public ResultT<LeaveRequest> RequestLeave(DateTime startDate, DateTime endDate, string? description, LeaveType leaveType )
        {
            if (Status == EmployeeStatus.Fired)
                return DomainErrors.Employee.InactiveEmployee;

            if (Status == EmployeeStatus.OnLeave)
                return DomainErrors.Employee.EmployeeOnLeave;

            if (leaveType is null)
                return DomainErrors.LeaveType.NullLeaveType;

            if (startDate > endDate)
                return DomainErrors.LeaveRequest.InvalidDateRange;

            if (startDate < DateTime.UtcNow)
                return DomainErrors.LeaveDays.PastDate;

            if (LeaveRequest.GetLeaveSpan(startDate, endDate) <= 0)
                return DomainErrors.LeaveRequest.InvalidDateRange;

            if (_requests.Any(r => r.OverlapsWith(startDate, endDate)))
                return DomainErrors.LeaveRequest.OverLappingRequest;

            var alloc = _allocations.FirstOrDefault(a => a.LeaveTypeId == leaveType.Id);
                
            if(alloc is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            if (_requests.Any(r =>
                r.LeaveTypeId == leaveType.Id &&
                r.StartDate == startDate &&
                r.EndDate == endDate && 
                r.IsPending()))
                return DomainErrors.LeaveRequest.RequestAlreadyExists;

            if (alloc.LeaveDays.Days < LeaveRequest.GetLeaveSpan(startDate, endDate)) 
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

            return ResultT<LeaveRequest>.Success(leaveRequest.Value);
        }
        private Result UpdateLeaveRequest(Guid requestId, Action<LeaveRequest> method)
        {
            var request = _requests.FirstOrDefault(r => r.Id == requestId);

            if (request is null)
                return DomainErrors.LeaveRequest.RequestNotFound;

            if (!request.IsPending())
                return DomainErrors.LeaveRequest.InvalidRequestStatus;

            method(request);

            return Result.Success();
        }
        public Result CancelLeaveRequest(Guid requestId) => UpdateLeaveRequest(requestId, r => r.Cancel());
        public Result RejectLeaveRequest(Guid requestId, string adminName) => UpdateLeaveRequest(requestId, r => r.Reject(adminName)); 
        public Result ApproveLeaveRequest(Guid requestId, string AdminName)
        {
            var request = _requests.FirstOrDefault(r => r.Id == requestId);

            if(request is null)
                return DomainErrors.LeaveRequest.RequestNotFound;

            if (request.StartDate.Year < DateTime.UtcNow.Year)
                return DomainErrors.LeaveRequest.InvalidYear;

            if (Status == EmployeeStatus.Fired)
                return DomainErrors.Employee.InactiveEmployee;

            var allocation = _allocations.FirstOrDefault(a => a.LeaveTypeId == request.LeaveTypeId && a.Year == request.StartDate.Year);

            if (allocation is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            if (!allocation.CanConsume(request.LeaveDays.Days))
                return DomainErrors.LeaveDays.InsufficientLeaveDays;

            var approved = request.Approve(AdminName);

            if (approved.isFailure)
                return approved.Error;

            allocation.Consume(request.LeaveDays.Days);

            return Result.Success();
        }
        public ResultT<LeaveAllocation> GetAllocationForLeaveType(Guid leaveTypeId)
        {
            var alloc = _allocations.FirstOrDefault(a => a.LeaveTypeId == leaveTypeId);

            if (alloc is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;

            return ResultT<LeaveAllocation>.Success(alloc);
        }
        public ResultT<LeaveRequest> HasPendingRequest(Guid leaveTypeId)
        {
            var request = _requests.FirstOrDefault(r => r.LeaveTypeId == leaveTypeId && r.IsPending());

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
        public Result Update(string? name, string? email)
        {
            if (!string.IsNullOrWhiteSpace(name))
                ChangeName(name);

            if (!string.IsNullOrWhiteSpace(email))
                ChangeEmail(email);

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(email))
                return DomainErrors.General.NullUpdateValues;

            return Result.Success();
        }
        public Result RemoveAllocation(Guid allocationId)
        {
            var allocation = _allocations.FirstOrDefault(a => a.Id == allocationId);
            if (allocation is null)
                return DomainErrors.LeaveAllocation.AllocationNotFound;
            _allocations.Remove(allocation);
            return Result.Success();
        }
        #endregion
        
        /*  raise domain events 
            EmployeeCreatedDomainEvent
            LeaveRequestedDomainEvent
            LeaveApprovedDomainEvent
            LeaveRejectedDomainEvent
            LeaveCancelledDomainEvent
            LeaveAllocatedDomainEvent
            add attribute for who processed leaves and allocation
        */
    }
}
