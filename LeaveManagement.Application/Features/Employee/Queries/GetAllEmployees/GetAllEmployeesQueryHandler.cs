using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.Employee.Queries.ListEmployees
{
    public sealed class GetAllEmployeesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllEmployeesQuery, List<EmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<EmployeeDto>>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
        {
            
        }
    }
}
