using FluentValidation;

namespace LeaveManagement.Application.Features.Employee.Commands.UpdateEmployee
{
    internal sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(c => c.EmployeeId)
                .NotEmpty();
            RuleFor(c => c.EmployeeName)
                .MinimumLength(5);
            RuleFor(c => c.Email)
                .EmailAddress();
        }
    }
}
