using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType
{
    public sealed record CreateLeaveTypeCommand(string Name, int DefaultDays) : ICommand<Guid>;
}
