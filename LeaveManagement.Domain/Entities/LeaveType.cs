using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Value_Objects;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

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

        public static ResultT<LeaveType> Create(string leaveName, int days)
        {
            if(string.IsNullOrWhiteSpace(leaveName))
                return DomainErrors.General.EmptyName;

            var LeaveName = Name.Create(leaveName);

            if (LeaveName.isFailure)
                return LeaveName.Error;

            if (leaveName == null)
                return DomainErrors.General.EmptyName;
            if (days <= 0)
                return DomainErrors.General.InvalidInt;

            var duration = LeaveDuration.Create(days);

            if (duration.isFailure)
                return DomainErrors.LeaveDays.InvalidLeaveDuration;

            return ResultT<LeaveType>.Success(new LeaveType(Guid.NewGuid(), LeaveName.Value, duration.Value));
        }

        public Result Update(string newName, int newDays)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return DomainErrors.General.EmptyName;

            var leaveName = Name.Create(newName);

            if (leaveName.isFailure)
                return leaveName.Error;

            if (newDays <= 0)
                return DomainErrors.General.InvalidInt;

            var duration = LeaveDuration.Create(newDays);

            if (duration.isFailure)
                return DomainErrors.LeaveDays.InvalidLeaveDuration;

            LeaveName = leaveName.Value;
            Days = duration.Value;

            return Result.Success();
        }
    }
}
