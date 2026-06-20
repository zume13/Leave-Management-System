using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.LogOut
{
    public class LogOutCommandValidator : AbstractValidator<LogOutCommand>
    {
        public LogOutCommandValidator()
        {
            RuleFor(x => x.employeeId)
                .NotEmpty().WithMessage("Employee id must not be empty");
        }
    }
}
