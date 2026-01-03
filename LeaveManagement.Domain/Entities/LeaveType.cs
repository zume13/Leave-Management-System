using LeaveManagement.Domain.Primitives;
using SharedKernel.Shared;
using LeaveManagement.Domain.Value_Objects;

namespace LeaveManagement.Domain.Entities
{
    public class LeaveType : Entity
    {
        private LeaveType(Guid id, Name name, LeaveDuration days) : base(id)  
        {
            LeaveName = name;
            Days = days;
        }
        private LeaveType() { }
        public Name LeaveName { get; private set; }
        public LeaveDuration Days { get; private set; }

        public static ResultT<LeaveType> Create(Name leaveName, int days)
        {
            if (leaveName == null)
                return DomainErrors.General.EmptyName;
            if (days <= 0)
                return DomainErrors.General.InvalidInt;

            var duration = LeaveDuration.Create(days);

            if (duration.isFailure)
                return DomainErrors.LeaveDays.InvalidLeaveDuration;

            return ResultT<LeaveType>.Success(new LeaveType(Guid.NewGuid(), leaveName, duration.Value));
        }
    }
}
