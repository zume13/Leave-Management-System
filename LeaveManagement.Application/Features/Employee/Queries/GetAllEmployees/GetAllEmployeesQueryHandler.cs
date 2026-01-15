using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;

namespace LeaveManagement.Application.Features.Employee.Queries.ListEmployees
{
    public sealed class GetAllEmployeesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllEmployeesQuery, List<EmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<EmployeeDto>>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
        {
            var employees = await (
                from e in _context.Employees.AsNoTracking()
                where e.Status != EmployeeStatus.Fired
                join d in _context.Departments.AsNoTracking()
                on e.DeptId equals d.Id
                select new EmployeeDto(
                    e.Id,
                    e.Name.Value,
                    e.Email.Value,
                    e.Status,
                    d.DepartmentName.Value
                )
            ).ToListAsync(cancellationToken);

            if (employees is null)
                return ApplicationErrors.Employee.NoEmployeesFound;

            return ResultT<List<EmployeeDto>>.Success(employees);
        }
    }
}
