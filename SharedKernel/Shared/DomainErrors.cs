
namespace SharedKernel.Shared
{
    public static class DomainErrors
    {
        public static class General
        {
            public static readonly Error StringTooLong = new("String.TooLong", "String is too long");
            public static readonly Error EmptyName = new("Name.Empty", "Name is empty");
            public static readonly Error InvalidInt = new("Int.Invalid", "Int must be positive");
            public static readonly Error InvalidGuid = new("Guid.Invalid", "Guid must be valid");
            public static readonly Error InvalidId = new("Id.Invalid", "ID must be positive");
            public static readonly Error NotFound = new("Object.NotFound", "Object was not found");
        }
        public static class Employee
        {
            public static readonly Error EmptyEmployeeName = new("EmployeeName.Empty", "Employee name is empty");
            public static readonly Error InactiveEmployee = new("Request.Invalid", "Inactive employees cannot execute functionalities");
            public static readonly Error NullEmployee = new("Employee.Null", "Passed employee instance was null");
            public static readonly Error EmployeeOnLeave = new("Employee.OnLeave", "Employee cannot access functionalities while on leave");
            public static readonly Error NullUserId = new("UserId.Null", "The user id is null");
        }

        public static class Email 
        {
            public static readonly Error EmptyEmail = new("Email.Empty", "Email is empty");
            public static readonly Error EmailInvalid = new("Email.Invalid", "Email is invalid");
        }

        public static class LeaveDays 
        {
            public static readonly Error InvalidLeaveDuration = new("Duration.Invalid", "Leave days should be greater than 0");
            public static readonly Error InvalidYear = new("Year.Invalid", "Year should be a valid year");
            public static readonly Error InvalidStartDate = new("StartDate.Invalid", "Start date should be in the present or before the end date");
            public static readonly Error InvalidEndDate = new("EndDate.Invalid", "End date should be in the present or after the start date");
            public static readonly Error FailedDeduction = new("Deduction.Failed", "Leave days deduction went wrong");
            public static readonly Error InsufficientLeaveDays = new("Insufficient.Days", "Insufficient Leave Days");
            public static readonly Error NullLeaveDays = new("LeaveDays.Null", "Passed days instance was null");
            public static readonly Error PastDate = new("Date.Invalid", "The date was in the past");
            public static readonly Error InvalidUsedDays = new("UsedDays.Invalid", "Employee cannot use more than allocated leave days");
        }

        public static class LeaveAllocation
        {
            public static readonly Error InstanceCreationFailed = new("Instantion.Failed", "LeaveAllocation creation failed");
            public static readonly Error AllocationNotFound = new("Alocation.Unexistent", "No allocation was found with the same type of the request");
            public static readonly Error DuplicateAllocation = new("Allocation.Duplicate", "One leave allocation per employee only");
        }

        public static class LeaveRequest
        {
            
            public static readonly Error InvalidRequestStatus = new("Request.Invalid", "Leave request status was invalid");
            public static readonly Error RequestAlreadyExists = new("Request.Duplicate", "Only one request per allocation");
            public static readonly Error OverLappingRequest = new("Request.Overlap", "Leave request has overlapping dates");
            public static readonly Error InvalidDateRange = new("Invalid.Dates", "Start date of request is past the end date");
            public static readonly Error InvalidEmployee = new("Employee.Invalid", "Request employee id and this employee are different");
            public static readonly Error NullLeaveRequest = new("Request.Null", "Passed leave request instance was null");
            public static readonly Error RequestNotFound = new("Request.Unexistent", "No request was found");
            public static readonly Error InvalidYear = new("Year.Invalid", "The input year has already passed");
        }

        public static class Department
        {
            public static readonly Error NulllDepartment = new("Department.Null", "Passed department instance was null");
        }

        public static class LeaveType
        {
            public static readonly Error NullLeaveType = new("LeaveType.Null", "Passed leave type instance was null");
        }
    }
}
