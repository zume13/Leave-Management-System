using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Value_Objects;
using System.Globalization;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Domain.Entities
{
    public class LeaveRequest : Entity
    {
        
        private LeaveRequest(Guid id, DateTime startDate, DateTime endDate, string description, Employee employee, LeaveType leaveType, LeaveDuration days, DateTime requestDate, DateTime modifiedDate) : base(id)
        {
            RequestDate = requestDate;
            LastModifiedDate = modifiedDate;
            ProcessedDate = null;
            StartDate = startDate;
            EndDate = endDate;
            LeaveDays = days;
            Status = LeaveRequestStatus.Pending; 
            Description = string.IsNullOrWhiteSpace(description) ? "No description provided" : description;
            EmployeeId = employee.Id;
            LeaveTypeId = leaveType.Id;
        }
        private LeaveRequest() { }
        public DateTime RequestDate { get; private set; }
        public DateTime LastModifiedDate { get; private set; }
        public DateTime? ProcessedDate { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public LeaveDuration LeaveDays {  get; private set; }
        public LeaveRequestStatus Status { get; private set; }
        public string? RejectionReason { get; private set; }
        public string? Description { get; private set; }
        public string? ProcessedBy { get; private set; }

        //FKs
        public Guid EmployeeId { get; private set; }
        public Guid LeaveTypeId { get; private set; }

        public static ResultT<LeaveRequest> Create(DateTime startDate, DateTime endDate, string? description, Employee employee, LeaveType leaveType)
        {

            int days = GetLeaveSpan(startDate, endDate); 

            var leaveDays = LeaveDuration.Create(days);

            if (leaveDays.isFailure)
                return DomainErrors.LeaveDays.InvalidLeaveDuration;

            if (startDate < DateTime.UtcNow)
                return DomainErrors.LeaveDays.InvalidStartDate;

            if (startDate > endDate)
                return DomainErrors.LeaveDays.InvalidStartDate;

            if (endDate < DateTime.UtcNow)
                return DomainErrors.LeaveDays.InvalidEndDate;

            if (employee is null)
                return DomainErrors.Employee.NullEmployee;

            if (leaveType is null)
                return DomainErrors.LeaveType.NullLeaveType;

            return ResultT<LeaveRequest>.Success(new LeaveRequest(Guid.NewGuid(), startDate, endDate, description, employee, leaveType, leaveDays.Value, DateTime.UtcNow, DateTime.UtcNow));
        }
        public Result EditLeaveRequest(DateTime newStartDate, DateTime newEndDate, string? newDescription)
        {
            if (Status != LeaveRequestStatus.Pending)
                return DomainErrors.LeaveRequest.InvalidRequestStatus;

            int days = GetLeaveSpan(newStartDate, newEndDate);

            var leaveDays = LeaveDuration.Create(days);

            if (leaveDays.isFailure)
                return DomainErrors.LeaveDays.InvalidLeaveDuration;

            if (newStartDate < DateTime.UtcNow)
                return DomainErrors.LeaveDays.InvalidStartDate;

            if (newStartDate > newEndDate)
                return DomainErrors.LeaveDays.InvalidStartDate;

            if (newEndDate < DateTime.UtcNow)
                return DomainErrors.LeaveDays.InvalidEndDate;

            StartDate = newStartDate;
            EndDate = newEndDate;
            LeaveDays = leaveDays.Value;
            Description = string.IsNullOrWhiteSpace(newDescription) ? "No description provided" : newDescription;
            LastModifiedDate = DateTime.UtcNow;

            return Result.Success();
        }
        public bool OverlapsWith(DateTime start, DateTime end)
        {
            return StartDate <= end && start <= EndDate;
        }
        public Result Approve(string AdminName)
        {
            if(Status != LeaveRequestStatus.Pending)
                return DomainErrors.LeaveRequest.InvalidRequestStatus;

            Status = LeaveRequestStatus.Approved;
            ProcessedDate = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;
            ProcessedBy = AdminName;

            return Result.Success();
        }
        public Result Cancel()
        {
            if(Status != LeaveRequestStatus.Pending)
                return DomainErrors.LeaveRequest.InvalidRequestStatus;

            Status = LeaveRequestStatus.Cancelled;
            ProcessedDate = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;

            return Result.Success();
        }
        public Result Reject(string adminName, string? reason = null)
        {
            if (Status != LeaveRequestStatus.Pending)
                return DomainErrors.LeaveRequest.InvalidRequestStatus;

            Status = LeaveRequestStatus.Rejected;
            RejectionReason = string.IsNullOrWhiteSpace(reason) ? "Not provided" : reason;
            ProcessedDate = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;
            ProcessedBy = adminName;

            return Result.Success();
        }
        public bool IsPending()
        {
            return Status == LeaveRequestStatus.Pending;
        }
        public bool IsApproved()
        {
            return Status == LeaveRequestStatus.Approved;
        }
        public bool IsRejected()
        {
            return Status == LeaveRequestStatus.Rejected;
        }
        public bool IsCancelled()
        {
            return Status == LeaveRequestStatus.Cancelled;
        }
        public static int GetLeaveSpan(DateTime startDate, DateTime endDate)
        {
            return (endDate.Date.AddDays(1) - startDate.Date).Days;
        }

    }
}
