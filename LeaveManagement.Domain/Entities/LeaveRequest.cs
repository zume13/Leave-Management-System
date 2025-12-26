using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Shared;
using LeaveManagement.Domain.Value_Objects;

namespace LeaveManagement.Domain.Entities
{
    public class LeaveRequest : Entity
    {
        
        internal LeaveRequest(Guid id, DateTime startDate, DateTime endDate, string description, Employee employee, LeaveType leaveType, LeaveDuration days) : base(id)
        {
            RequestDate = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;
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

        public string? Description { get; private set; }

        //FKs
        public Guid EmployeeId { get; private set; }
        public Guid LeaveTypeId { get; private set; }

        // Nav Properties
        public Employee? Employee { get ; private set; }
        public LeaveType? LeaveType { get; private set; }

        public static ResultT<LeaveRequest> Create(DateTime startDate, DateTime endDate, string? description, Employee employee, LeaveType leaveType)
        {

            int days = (endDate.Date.AddDays(1) - startDate.Date).Days;

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
                return DomainErrors.General.NullObject;

            if (leaveType is null)
                return DomainErrors.General.NullObject;

            return ResultT<LeaveRequest>.Success(new LeaveRequest(Guid.NewGuid(), startDate, endDate, description, employee, leaveType, leaveDays.Value));
        }

        public ResultT<bool> Approve()
        {
            if (Status != LeaveRequestStatus.Pending)
                return DomainErrors.LeaveRequest.InvalidRequestStatus;

            Status = LeaveRequestStatus.Approved;
            return ResultT<bool>.Success(true);
        }

        public bool OverlapsWith(DateTime start, DateTime end)
        {
            return StartDate <= end && start <= EndDate;
        }


    }
}
