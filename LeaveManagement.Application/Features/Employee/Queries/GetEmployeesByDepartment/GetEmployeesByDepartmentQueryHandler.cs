using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Constants;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;


namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment
{
    internal sealed class GetEmployeesByDepartmentQueryHandler(IApplicationDbContext context) : IQueryHandler<GetEmployeesByDepartmentQuery, List<GetEmployeesByDepartmentDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetEmployeesByDepartmentDto>>> Handle(GetEmployeesByDepartmentQuery query, CancellationToken cancellationToken)
        {
            int pageSize = query.pageSize <= 0 ? NumericConstant.DefaultPageSize : NumericConstant.MaxPageSize(query.pageSize);
            int PageNumber = Math.Max(1, query.pageNumber);

            var employees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.DeptId == query.deptId)
                .OrderBy(e => e.Name.Value)
                .Skip((PageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .OrderBy(e => e.Name.Value)
                .Select(e => new GetEmployeesByDepartmentDto(
                    e.Id,
                    e.Name.Value,
                    e.Email.Value,
                    e.Status))
                .ToListAsync(cancellationToken);

            if (employees.Count == 0)
                return ApplicationErrors.Employee.NoEmployeesFound;

            return ResultT<List<GetEmployeesByDepartmentDto>>.Success(employees);
        }
    }
} 
