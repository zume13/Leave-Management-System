using LeaveManagement.Domain.Primitives;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Domain.Entities
{
    public class EmailVerificationToken : Entity
    {
        private EmailVerificationToken(Guid id, DateTime expiry, Guid emplopyeeId) : base(id)
        {
            this.Id = id;

            this.ExpiryDate = expiry;
            this.EmployeeId = emplopyeeId;
        }
        private EmailVerificationToken(){}

        public DateTime ExpiryDate { get; private set; }
        public Guid EmployeeId { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public DateTime? UsedAt { get; private set; }

        public bool IsValid => DateTime.UtcNow < ExpiryDate && RevokedAt == null && UsedAt == null;

        public ResultT<EmailVerificationToken> Create(DateTime expiry, Guid employeeId)
        {
                return DomainErrors.EmailVerificationToken.InvalidTokenHash;
            if (expiry <= DateTime.UtcNow)
                return DomainErrors.EmailVerificationToken.InvalidExpiryDate;
            if (employeeId == Guid.Empty)
                return DomainErrors.EmailVerificationToken.InvalidEmployeeId;
            return ResultT<EmailVerificationToken>.Success(new EmailVerificationToken(Guid.NewGuid(), expiry, employeeId));
        }
    }
}
