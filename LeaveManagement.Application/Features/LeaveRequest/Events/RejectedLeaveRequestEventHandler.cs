using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Services;
using LeaveManagement.Domain.Events.LeaveRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.DomainEvents;


namespace LeaveManagement.Application.Features.LeaveRequest.Events
{
    public class RejectedLeaveRequestEventHandler(IApplicationDbContext _context, IEmailService _service) : IDomainEventHandler<RejectedLeaveEvent>
    {
        public async Task Handle(RejectedLeaveEvent domainEvent, CancellationToken ct = default)
        {
                var request = await _context.LeaveRequests.Where(r => r.Id == domainEvent.requestId)
                .FirstOrDefaultAsync(ct);

                if (request is null)
                    throw new InvalidOperationException($"Leave request with an id of {domainEvent.requestId} was not found");

                var leaveType = await _context.LeaveTypes.Where(lt => lt.Id == request.LeaveTypeId)
                .FirstOrDefaultAsync(ct);
                
                if(leaveType is null)
                    throw new InvalidOperationException($"Leave type with an id of {request.LeaveTypeId} was not found");

                var success = await _service.SendLeavedRejectedEmailAsync(domainEvent.employeeName, domainEvent.employeeEmail, leaveType.LeaveName.Value, domainEvent.rejectionReason, ct);

                if (success.isFailure)
                    throw new InvalidOperationException("There was an error sending the email. Error: " + success.Error);
        }
    }
}
