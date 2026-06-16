using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    public sealed record CreateLeaveRequestCommand(DateTime startDate, DateTime endDate, string? description, Guid employeeId, Guid leaveTypeId) : ICommand<Guid>;
}
