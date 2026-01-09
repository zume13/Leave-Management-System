using LeaveManagement.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation
{
    public record DeleteLeaveAllocationCommand(Guid EmployeeId, Guid AllocationId) : ICommand<bool>;
}
