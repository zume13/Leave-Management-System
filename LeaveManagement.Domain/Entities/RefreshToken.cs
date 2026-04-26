using LeaveManagement.Domain.Primitives;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Domain.Entities
{
    public class RefreshToken : Entity
    {
        protected RefreshToken() { }

        private RefreshToken(Guid id, string token, DateTime expiresAt, Guid employeeid) : base(id)
        {
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
            EmployeeId = employeeid;
            IsRevoked = false;
        }


        public string Token { get; private set; } = null!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid EmployeeId { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? ReplacedByToken { get; private set; }

        public static ResultT<RefreshToken> Create(Guid id, string token, DateTime expiresAt, Guid employeeid)
        {
            if (string.IsNullOrWhiteSpace(token))
                return DomainErrors.RefreshToken.NullToken;

            if (expiresAt <= DateTime.UtcNow)
                return DomainErrors.RefreshToken.TokenExpired;

            return ResultT<RefreshToken>.Success(new RefreshToken(id, token, expiresAt, employeeid));
        }
        public Result Revoke(string? replacedByToken = null)
        {
            if (IsRevoked)
                return DomainErrors.RefreshToken.RevokedToken;

            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
            ReplacedByToken = replacedByToken;

            return Result.Success();
        }
        public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive() => !IsRevoked && !IsExpired();
    }
}
