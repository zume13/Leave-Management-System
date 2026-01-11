using LeaveManagement.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.Employee.Commands.RemoveEmployee
{
    public sealed record RemoveEmployeeCommand(Guid employeeId) : ICommand<bool>;
}
