using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using SharedKernel.Shared;
using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeave
{
    public sealed class CreateLeaveCommandHandler(IApplicationDbContext context) : ICommandHandler<CreateLeaveCommand, Guid>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<Guid>> Handle(CreateLeaveCommand command, CancellationToken token = default)
        {
            var result = LeaveType.Create(command.Name, command.DefaultDays);

            if (result.isFailure)
                return result.Error;

            await _context.LeaveTypes.AddAsync(result.Value, token);

            return ResultT<Guid>.Success(result.Value.Id);
        }
    }
}
