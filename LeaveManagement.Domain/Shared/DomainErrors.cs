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
            public static Error InactiveEmployeeRequest = new("Request.Invalid", "Inactive employees cannot request for leaves");
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
            public static Error InstanceCreationFailed = new("Instantion.Failed", "LeaveAllocation creation failed");
            public static Error AllocationNotFound = new("Alocation.Unexistent", "No allocation was found with the same type of the request");
            public static Error DuplicateAllocation = new("Allocation.Duplicate", "One leave allocation per employee only");
        }

        public static class LeaveRequest
        {
            
            public static Error InvalidRequestStatus = new("Request.Invalid", "Leave request status was invalid");
            public static Error RequestAlreadyExists = new("Request.Duplicate", "Only one request per allocation");
            public static Error OverLappingRequest = new("Request.Overlap", "Leave request has overlapping dates");
            public static Error InvalidDateRange = new("Invalid.Dates", "Start date of request is past the end date");
            public static Error InvalidEmployee = new("Employee.Invalid", "Request employee id and this employee are different");
        }
    }
}
