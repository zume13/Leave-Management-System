using LeaveManagement.Application.Abstractions.Data;
using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Constants;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployeeByName;
using LeaveManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Shared.Result;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployeeByName
{
    internal class GetEmployeeByNameQueryHandler(IApplicationDbContext _context) : IQueryHandler<GetEmployeeByNameQuery, List<EmployeeDto>>
    {
        public async Task<ResultT<List<EmployeeDto>>> Handle(GetEmployeeByNameQuery query, CancellationToken cancellationToken = default)
        {
            int pageNumber = Math.Max(1, query.PageNumber);
            int pageSize = query.PageSize < 1 ? NumericConstant.DefaultPageSize : NumericConstant.MaxPageSize(query.PageSize);

            var employees = await _context.Employees
                            .AsNoTracking()
                            .Where(e => e.Status == EmployeeStatus.Active)
                            .Where(e => EF.Functions.Like(e.Name.Value, $"%{query.Name}%"))
                            .OrderBy(e => e.Name.Value)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Join(
                                _context.Departments,
                                e => e.DeptId,
                                d => d.Id,
                                (e, d) => new EmployeeDto(
                                    e.Id,
                                    e.Name.Value,
                                    e.Email.Value,
                                    e.Status.ToString(),
                                    d.DepartmentName.Value))
                            .ToListAsync(cancellationToken);

            return ResultT<List<EmployeeDto>>.Success(employees);
        }
    }
}
