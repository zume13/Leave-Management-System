using LeaveManagement.SharedKernel.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Domain.Events.Employees
{
    public record MemberRegisteredEvent(string EmployeeName, string EmployeeEmail, string VerificationToken) : IDomainEvent;
}
