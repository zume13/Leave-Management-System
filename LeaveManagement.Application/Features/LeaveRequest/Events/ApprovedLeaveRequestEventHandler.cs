using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Domain.Events.LeaveRequest;
using SharedKernel.DomainEvents;

namespace LeaveManagement.Application.Features.LeaveRequest.Events
{
    public class ApprovedLeaveRequestEventHandler(IEmailService _service, IApplicationDbContext _context) : IDomainEventHandler<ApprovedLeaveEvent>
    {
        public async Task Handle(ApprovedLeaveEvent domainEvent, CancellationToken ct = default)
        {
            var request = await _context.LeaveRequests.FindAsync(domainEvent.requestId, ct);

            if(request is null)
                throw new InvalidOperationException($"Leave request with an id of {domainEvent.requestId} was not found");

            var leaveType = await _context.LeaveTypes.FindAsync(request.LeaveTypeId, ct);

            if(leaveType is null)
                throw new InvalidOperationException($"Leave type with an id of {request.LeaveTypeId} was not found");

            var message = await _service.SendLeaveApprovedEmailAsync(domainEvent.employeeName, domainEvent.employeeEmail, leaveType.LeaveName.Value, ct);

            if(message.isFailure)
                throw new InvalidOperationException("There was an error sending the email. Error: " + message.Error);
        }
    }
}
