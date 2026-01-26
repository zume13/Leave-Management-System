using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Domain.Events.Employees;
using SharedKernel.DomainEvents;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Events
{
    public class SendlVerificationEmailHandler(IEmailService _service) : IDomainEventHandler<MemberRegisteredEvent>
    {
        public async Task Handle(MemberRegisteredEvent domainEvent, CancellationToken ct = default)
        {
            await _service.SendEmailVerificationAsync(domainEvent.EmployeeName, domainEvent.EmployeeEmail, domainEvent.VerificationToken);
        }
    }
}
