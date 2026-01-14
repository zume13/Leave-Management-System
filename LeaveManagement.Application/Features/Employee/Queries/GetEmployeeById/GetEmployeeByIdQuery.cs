using LeaveManagement.Application.Abstractions.Messaging;
using LeaveManagement.Application.Dto.Response.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.Employee.Queries.GetEmployee
{
    public sealed record GetEmployeeByIdQuery(Guid EmployeeId) : IQuery<EmployeeDto>;

}
