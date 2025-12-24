using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Shared
{
    internal class DomainErrors
    {
        public static class General
        {
            public static Error StringTooLong = new("String.TooLong", "String is too long");
            public static Error EmptyName = new("Name.Empty", "Name is empty");
            public static Error InvalidInt = new("Int.Invalid", "Int must be positive");
            public static Error InvalidGuid = new("Guid.Invalid", "Guid must be valid");
            public static Error InvalidId = new("Id.Invalid", "ID must be positive");
            public static Error NullObject = new("Object.Null", "Provide an not null object");
            public static Error NotFound = new("Object.NotFound", "Object was not found");
        }
        public static class Employee
        {
            public static Error EmptyEmployeeName = new("EmployeeName.Empty", "Employee name is empty");
        }

        public static class Email 
        {
            public static Error EmptyEmail = new("Email.Empty", "Email is empty");
            public static Error EmailInvalid = new("Email.Invalid", "Email is invalid");
        }

        public static class LeaveDays 
        {
            public static Error InvalidLeaveDuration = new("Duration.Invalid", "Leave days should be greater than 0");
            public static Error InvalidYear = new("Year.Invalid", "Year should be a valid year");
            public static Error InvalidStartDate = new("StartDate.Invalid", "Start date should be in the present or before the end date");
            public static Error InvalidEndDate = new("EndDate.Invalid", "End date should be in the present or after the start date");
            public static Error FailedDeduction = new("Deduction.Failed", "Leave days deduction went wrong");
            public static Error InsufficientLeaveDays = new("Insufficient.Days", "Insufficient Leave Days");
        }

        public static class LeaveAllocation
        {
            public static Error InstanceCreationFailed = new("Instantio.Failed", "LeaveAllocation creation failed");
        }
    }
}
