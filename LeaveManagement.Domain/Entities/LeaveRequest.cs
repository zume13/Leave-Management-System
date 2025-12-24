

using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Shared;
using LeaveManagement.Domain.Value_Objects;

namespace LeaveManagement.Domain.Entities
{
    public class LeaveRequest : Entity
    {
        
        internal LeaveRequest(Guid id, DateTime startDate, DateTime endDate, string description, Employee employee, LeaveType leaveType) : base(id)
        {
            RequestDate = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;
            ProcessedDate = null;
            StartDate = startDate;
            EndDate = endDate;
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
        public LeaveRequestStatus Status { get; private set; }

        public string? Description { get; private set; }

        //FKs
        public Guid EmployeeId { get; private set; }
        public Guid LeaveTypeId { get; private set; }

        // Nav Properties
        public Employee? Employee { get ; private set; }
        public LeaveType? LeaveType { get; private set; }

        public static ResultT<LeaveRequest> Create(Guid id, DateTime startDate, DateTime endDate, string? description, Employee employee, LeaveType leaveType)
        {
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

            return ResultT<LeaveRequest>.Success(new LeaveRequest(Guid.NewGuid(), startDate, endDate, description, employee, leaveType));
        }
    }
}
