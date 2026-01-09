using LeaveManagement.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.AllocateLeave
{
    public record AllocateLeaveCommand(Guid EmployeeId, Guid LeaveTypeId) : ICommand<Guid>;
}
