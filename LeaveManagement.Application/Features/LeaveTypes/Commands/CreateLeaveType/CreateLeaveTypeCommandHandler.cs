using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType
{
    public sealed class CreateLeaveTypeCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateLeaveTypeCommand, Guid>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<Guid>> Handle(CreateLeaveTypeCommand command, CancellationToken token = default)
        {
            var result = Domain.Entities.LeaveType.Create(command.Name, command.DefaultDays);

            if (result.isFailure)
                return result.Error;

            await _context.LeaveTypes.AddAsync(result.Value, token);

            return ResultT<Guid>.Success(result.Value.Id);
        }
    }
}
