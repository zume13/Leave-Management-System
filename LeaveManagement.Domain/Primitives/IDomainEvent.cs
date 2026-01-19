using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Domain.Primitives
{
    public interface IDomainEvent
    {
        DateTime OccuredON { get; }
    }
}
