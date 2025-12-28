using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Shared;

namespace LeaveManagement.Domain.Value_Objects
{
    public class LeaveDuration : ValueObject
    {
        public int Days { get; }
        protected LeaveDuration(int days) 
        {
            Days = days;
        }
        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Days;
        }

        public static ResultT<LeaveDuration> Create(int days)
        {
            if (days <= 0)
                return DomainErrors.LeaveDays.InvalidLeaveDuration;

            return ResultT<LeaveDuration>.Success(new LeaveDuration(days));
        }

        public ResultT<LeaveDuration> Deduct(int daysToDeduct)
        {
            if(daysToDeduct <= 0)
                return DomainErrors.General.InvalidInt;
            
            if(daysToDeduct > Days)
                return DomainErrors.LeaveDays.InsufficientLeaveDays;

            return ResultT<LeaveDuration>.Success(new LeaveDuration(Days - daysToDeduct));
        }
    }
}
