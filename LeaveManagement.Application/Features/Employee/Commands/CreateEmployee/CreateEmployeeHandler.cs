
using LeaveManagement.Domain.Commons.Contracts;
using LeaveManagement.Domain.Shared;

namespace LeaveManagement.Application.Features.Employee.Commands.CreateEmployee
{
    public class CreateEmployeeHandler
    {
        private readonly IEmployeeRepository _employeeRepository;

        public CreateEmployeeHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Result<CreateEmployeeDto> Handle(CreateEmployeeCommand command, CancellationToken ct)
        {

        }
    }
}
