using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeave;
using LeaveManagement.Application.Features.LeaveTypes.Commands.RemoveLeave;
using LeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeave;

namespace LeaveManagement.API.Handlers.LeaveType
{
    public class TypeCommandHandlers
    {
        public ICommandHandler<CreateLeaveCommand, Guid> Create { get; }
        public ICommandHandler<RemoveLeaveTypeCommand, bool> Remove { get; }
        public ICommandHandler<UpdateLeaveTypeCommand, Guid> Update { get; }

        public TypeCommandHandlers(
            ICommandHandler<CreateLeaveCommand, Guid> create,
            ICommandHandler<RemoveLeaveTypeCommand, bool> remove,
            ICommandHandler<UpdateLeaveTypeCommand, Guid> update)
        {
            Create = create;
            Remove = remove;
            Update = update;
        }
    }
}
