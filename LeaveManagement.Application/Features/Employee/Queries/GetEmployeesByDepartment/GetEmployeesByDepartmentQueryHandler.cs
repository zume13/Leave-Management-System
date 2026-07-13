using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Constants;
using LeaveManagement.Application.Dto.Response.Employee;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;


namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment
{
    internal sealed class GetEmployeesByDepartmentQueryHandler(IApplicationDbContext context) : IQueryHandler<GetEmployeesByDepartmentQuery, List<EmployeeDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<EmployeeDto>>> Handle(GetEmployeesByDepartmentQuery query, CancellationToken cancellationToken)
        {
            int pageSize = query.pageSize <= 0 ? NumericConstant.DefaultPageSize : NumericConstant.MaxPageSize(query.pageSize);
            int PageNumber = Math.Max(1, query.pageNumber);

            var employees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.DeptId == query.deptId)
                .Join(_context.Departments.AsNoTracking(),
                      e => e.DeptId,
                      d => d.Id,
                      (e, d) => new {Employee = e, Department = d} )
                .OrderBy(e => e.Employee.Name.Value)
                .Skip((PageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .Select(e => new EmployeeDto(
                    e.Employee.Id,
                    e.Employee.Name.Value,
                    e.Employee.Email.Value,
                    e.Employee.Status.ToString(),
                    e.Department.DepartmentName.Value
                 ))
                .ToListAsync(cancellationToken);

            if (employees.Count == 0)
                return ApplicationErrors.Employee.NoEmployeesFound;

            return ResultT<List<EmployeeDto>>.Success(employees);
        }
    }
} 
