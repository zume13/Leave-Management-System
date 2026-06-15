using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.RotateRefreshToken
{
    internal class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.refreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MaximumLength(500).WithMessage("Refresh token must not exceed 500 characters.");
        }
    }
}
