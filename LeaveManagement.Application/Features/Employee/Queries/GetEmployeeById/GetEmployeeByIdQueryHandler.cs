using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;


namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployee
{
    internal sealed class GetEmployeeByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        IApplicationDbContext _context = context;
        public async Task<ResultT<EmployeeDto>> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            int pageSize = query.pageSize <= 0 ? 20 : Math.Min(query.pageSize, 50);
            int pageNumber = query.pageNumber <= 0 ? 1 : query.pageNumber;

            var employee = await _context.Employees
                .AsNoTracking()
                .Where(e => e.Id == query.EmployeeId)
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
                .FirstOrDefaultAsync(cancellationToken);

            if (employee is null)
                return ApplicationErrors.Employee.EmployeeNotFound(query.EmployeeId);

            return ResultT<EmployeeDto>.Success(employee);
        }
    }
}
