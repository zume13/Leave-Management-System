using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Domain.Events.Employees;
using SharedKernel.DomainEvents;

namespace LeaveManagement.Application.Features.LeaveRequest.Events
{
    internal class ApprovedLeaveRequestEventHandler(IEmailService _service) : IDomainEventHandler<MemberRegisteredEvent>
    {
        public async Task Handle(MemberRegisteredEvent domainEvent, CancellationToken ct = default)
        {
            await _service.SendLeaveApprovedEmailAsync(domainEvent.EmployeeName, domainEvent.EmployeeEmail, domainEvent.Admin!, ct);
        }
    }
}
