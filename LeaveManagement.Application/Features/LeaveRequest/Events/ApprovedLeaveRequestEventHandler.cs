using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Domain.Events.Employees;
using LeaveManagement.Domain.Events.LeaveRequest;
using SharedKernel.DomainEvents;

namespace LeaveManagement.Application.Features.LeaveRequest.Events
{
    internal class ApprovedLeaveRequestEventHandler(IEmailService _service) : IDomainEventHandler<ApprovedLeaveEvent>
    {
        public async Task Handle(ApprovedLeaveEvent domainEvent, CancellationToken ct = default)
        {
            await _service.SendLeaveApprovedEmailAsync(domainEvent.EmployeeName, domainEvent.EmployeeEmail, domainEvent.Admin!, ct);
        }
    }
}
