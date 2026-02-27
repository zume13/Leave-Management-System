using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Domain.Enums;
using LeaveManagement.Domain.Value_Objects;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Queries.ListEmployees
{
    public sealed class GetAllEmployeesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetAllEmployeesQuery, List<EmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<EmployeeDto>>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
        {
            int pageSize = query.pageSize <= 0 ? 20 : Math.Min(query.pageSize, 50);
            int pageNumber = query.pageNumber <= 0 ? 1 : query.pageNumber;

            var employees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.Status != EmployeeStatus.Fired)
                .OrderBy(e => e.Name.Value)
                .Skip((query.pageNumber - 1) * query.pageSize)
                .Take(query.pageSize)
                .Join(_context.Departments.AsNoTracking(), 
                    e => e.DeptId,
                    d => d.Id,
                    (e, d) => new EmployeeDto(
                        e.Id,
                        e.Name.Value,
                        e.Email.Value,
                        e.Status,
                        d.DepartmentName.Value))
                .ToListAsync(cancellationToken);

            if (employees.Count == 0)
                return ApplicationErrors.Employee.NoEmployeesFound;

            return ResultT<List<EmployeeDto>>.Success(employees);
        }
    }
}
