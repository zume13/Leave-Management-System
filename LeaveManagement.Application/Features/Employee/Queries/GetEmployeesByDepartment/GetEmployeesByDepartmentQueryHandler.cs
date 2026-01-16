using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared;


namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment
{
    internal sealed class GetEmployeesByDepartmentQueryHandler(IApplicationDbContext context) : IQueryHandler<GetEmployeesByDepartmentQuery, List<GetEmployeesByDepartmentDto>>
    {
        private readonly IApplicationDbContext _context = context;
        public async Task<ResultT<List<GetEmployeesByDepartmentDto>>> Handle(GetEmployeesByDepartmentQuery query, CancellationToken cancellationToken)
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .Where(e => e.DeptId == query.DeptId)
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
