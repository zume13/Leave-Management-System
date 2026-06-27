using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Constants;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Domain.Enums;
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
            int pageSize = query.pageSize <= 0 ? NumericConstant.DefaultPageSize : NumericConstant.MaxPageSize(query.pageSize);
            int pageNumber = Math.Max(1, query.pageNumber);

            var employees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.Status == EmployeeStatus.Active)
                .OrderBy(e => e.Name.Value)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
