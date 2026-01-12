using LeaveManagement.Application.Abstractions.Messaging;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeave
{
    public sealed record CreateLeaveCommand(string Name, int DefaultDays) : ICommand<Guid>;
}
