
using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Shared;
using LeaveManagement.Domain.Value_Objects;

namespace LeaveManagement.Domain.Entities
{
    public class LeaveType : Entity
    {
        internal LeaveType(Guid id, Name name, LeaveDuration days) : base(id)  
        {
            LeaveName = name;
            Days = days;
            Period = DateTime.UtcNow.Year;
        }
        private LeaveType() { }
        public Name LeaveName { get; private set; }
        public LeaveDuration Days { get; private set; }
        public int Period { get; private set; }

        public static ResultT<LeaveType> Create(Name leaveName, LeaveDuration days, DateTime year)
        {
            if (leaveName == null)
                return DomainErrors.General.EmptyName;
            if (days == null || days.Days <= 0)
                return DomainErrors.LeaveDays.InvalidLeaveDuration;

            return ResultT<LeaveType>.Success(new LeaveType(Guid.NewGuid(), leaveName, days));
        }
    }
}
