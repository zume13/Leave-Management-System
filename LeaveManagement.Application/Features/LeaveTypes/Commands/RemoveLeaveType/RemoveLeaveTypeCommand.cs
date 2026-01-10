using LeaveManagement.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Features.LeaveTypes.Commands.RemoveLeaveType
{
    public record RemoveLeaveTypeCommand(Guid LeaveTypeId) : ICommand<bool>;
}
