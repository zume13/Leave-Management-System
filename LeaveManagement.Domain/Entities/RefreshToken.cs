using LeaveManagement.Domain.Primitives;
using SharedKernel.Shared;
using System;

namespace LeaveManagement.Domain.Entities
{
    public class RefreshToken : Entity
    {
        protected RefreshToken() { }

        private RefreshToken(Guid id, string token, DateTime expiresAt, Guid userId) : base(id)
        {
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
            UserId = userId;
            IsRevoked = false;
        }


        public string Token { get; private set; } = null!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? ReplacedByToken { get; private set; }

        public Guid UserId { get; private set; }

        public static ResultT<RefreshToken> Create(string token, DateTime expiresAt, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(token))
                return DomainErrors.RefreshToken.NullToken;

            if (expiresAt <= DateTime.UtcNow)
                return DomainErrors.RefreshToken.TokenExpired;

            return ResultT<RefreshToken>.Success(new RefreshToken(Guid.NewGuid(), token, expiresAt, userId));
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
