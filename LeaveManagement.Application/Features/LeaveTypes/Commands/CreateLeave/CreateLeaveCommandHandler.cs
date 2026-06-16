using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Domain.Entities;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeave
{
    public sealed class CreateLeaveCommandHandler(IApplicationDbContext _context) : ICommandHandler<CreateLeaveCommand, Guid>
    {
        public async Task<ResultT<Guid>> Handle(CreateLeaveCommand command, CancellationToken token = default)
        {
            var result = LeaveType.Create(command.Name, command.DefaultDays);

            if (result.isFailure)
                return result.Error;

            await _context.LeaveTypes.AddAsync(result.Value, token);


            await _context.SaveChangesAsync(token);

            return ResultT<Guid>.Success(result.Value.Id);
        }
    }
}
    