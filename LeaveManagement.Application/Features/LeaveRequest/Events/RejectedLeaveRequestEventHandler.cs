using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Domain.Events.LeaveRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.DomainEvents;


namespace LeaveManagement.Application.Features.LeaveRequest.Events
{
    public class RejectedLeaveRequestEventHandler(IApplicationDbContext _context, ILogger _logger) : IDomainEventHandler<RejectedLeaveEvent>
    {
        public async Task Handle(RejectedLeaveEvent domainEvent, CancellationToken ct = default)
        {
            try
            {
                var request = await _context.LeaveRequests.Where(r => r.Id == domainEvent.requestId)
                .FirstOrDefaultAsync(ct);

                var leaveType = await _context.LeaveTypes.Where(lt => lt.Id == request!.LeaveTypeId)
                .FirstOrDefaultAsync(ct);
            }catch(Exception ex)
            {

            }
      
            await 
        }
    }
}
