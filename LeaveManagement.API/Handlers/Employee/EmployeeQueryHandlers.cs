using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployee;
using LeaveManagement.Application.Features.Employee.Queries.GetEmployeesByDepartment;
using LeaveManagement.Application.Features.Employee.Queries.ListEmployees;

namespace LeaveManagement.API.Handlers.Employee
{
    public class EmployeeQueryHandlers
    {
        public IQueryHandler<GetAllEmployeesQuery, List<EmployeeDto>> GetAll { get; }
        public IQueryHandler<GetEmployeeByIdQuery, EmployeeDto> GetById { get; }
        public IQueryHandler<GetEmployeesByDepartmentQuery, List<GetEmployeesByDepartmentDto>> GetByDepartment { get; }

        public EmployeeQueryHandlers(
            IQueryHandler<GetAllEmployeesQuery, List<EmployeeDto>> _GetAll,
            IQueryHandler<GetEmployeeByIdQuery, EmployeeDto> _GetById,
            IQueryHandler<GetEmployeesByDepartmentQuery, List<GetEmployeesByDepartmentDto>> _GetByDepartment)
        {
            GetAll = _GetAll;
            GetById = _GetById;
            GetByDepartment = _GetByDepartment;

        }
    }
}
