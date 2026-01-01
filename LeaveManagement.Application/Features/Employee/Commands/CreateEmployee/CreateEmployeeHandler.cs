using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Domain.Shared;

namespace LeaveManagement.Application.Features.Employee.Commands.CreateEmployee
{
    public class CreateEmployeeHandler : ICommandHandler<CreateEmployeeCommand, CreateEmployeeDto>
    {
        private readonly IApplicationDbContext _context;

        public CreateEmployeeHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResultT<CreateEmployeeDto>> Handle(CreateEmployeeCommand command, CancellationToken token)
        {
             
        }
    }
}
