using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeave;
using LeaveManagement.Application.Features.LeaveTypes.Commands.RemoveLeave;
using LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeave;

namespace LeaveManagement.API.Handlers.LeaveType
{
    public class TypeCommandHandlers
    {
        public ICommandHandler<CreateLeaveCommand, Guid> Create { get; }
        public ICommandHandler<RemoveLeaveTypeCommand> Remove { get; }
        public ICommandHandler<UpdateLeaveTypeCommand> Update { get; }

        public TypeCommandHandlers(
            ICommandHandler<CreateLeaveCommand, Guid> create,
            ICommandHandler<RemoveLeaveTypeCommand> remove,
            ICommandHandler<UpdateLeaveTypeCommand> update)
        {
            Create = create;
            Remove = remove;
            Update = update;
        }
    }
}
