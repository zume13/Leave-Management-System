using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Commands.Promote
{
    public sealed class PromoteCommandHandler(IApplicationDbContext _context) : ICommandHandler<PromoteCommand, PromoteEmployeeDto>
    {
        public async Task<ResultT<PromoteEmployeeDto>> Handle(PromoteCommand command, CancellationToken token)
        {
            var employee = await _context.Employees.FindAsync(command.employeeId, token);  

            if(employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound(command.employeeId);
            
            var isSuccess = employee.Promote(command.role);

            if(isSuccess.isFailure)
                return ResultT<PromoteEmployeeDto>.Failure(isSuccess.Error);

            await _context.SaveChangesAsync(token);

            return ResultT<PromoteEmployeeDto>.Success(new PromoteEmployeeDto(command.employeeId, command.role));
        }
    }
}
