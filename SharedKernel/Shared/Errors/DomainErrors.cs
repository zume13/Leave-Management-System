using SharedKernel.Shared.Result;

namespace SharedKernel.Shared.Errors
{
    public static class DomainErrors
    {
        public static class General
        {
            public static readonly Error StringTooLong = Error.Validation("String.TooLong", "String is too long");
            public static readonly Error EmptyName = Error.Validation("Name.Empty", "Name is empty");
            public static readonly Error InvalidInt = Error.Validation("Int.Invalid", "Int must be positive");
            public static readonly Error InvalidGuid = Error.Validation("Guid.Invalid", "Guid must be valid");
            public static readonly Error InvalidId = Error.Validation("Id.Invalid", "ID must be positive");
            public static readonly Error NotFound = Error.NotFound("Object.NotFound", "Object was not found");
            public static readonly Error NullUpdateValues = Error.Validation("Values.Null", "Provided details were null");
        }
        public static class Employee
        {
            public static readonly Error EmptyEmployeeName = Error.Validation("EmployeeName.Empty", "Employee name is empty");
            public static readonly Error InactiveEmployee = Error.Problem("Request.Invalid", "Inactive employees cannot execute functionalities");
            public static readonly Error NullEmployee = Error.Validation("Employee.Null", "Passed employee instance was null");
            public static readonly Error EmployeeOnLeave = Error.Problem("Employee.OnLeave", "Employee cannot access functionalities while on leave");
            public static readonly Error NullUserId = Error.Validation("UserId.Null", "The user id is null");
        }

        public static class Email
        {
            public static readonly Error EmptyEmail = Error.Validation("Email.Empty", "Email is empty");
            public static readonly Error EmailInvalid = Error.Validation("Email.Invalid", "Email is invalid");
        }

        public static class LeaveDays
        {
            public static readonly Error InvalidLeaveDuration = Error.Validation("Duration.Invalid", "Leave days should be greater than 0");
            public static readonly Error InvalidYear = Error.Validation("Year.Invalid", "Year should be a valid year");
            public static readonly Error InvalidStartDate = Error.Validation("StartDate.Invalid", "Start date should be in the present or before the end date");
            public static readonly Error InvalidEndDate = Error.Validation("EndDate.Invalid", "End date should be in the present or after the start date");
            public static readonly Error FailedDeduction = Error.Problem("Deduction.Failed", "Leave days deduction went wrong");
            public static readonly Error InsufficientLeaveDays = Error.Problem("Insufficient.Days", "Insufficient Leave Days");
            public static readonly Error NullLeaveDays = Error.Validation("LeaveDays.Null", "Passed days instance was null");
            public static readonly Error PastDate = Error.Problem("Date.Invalid", "The date was in the past");
            public static readonly Error InvalidUsedDays = Error.Validation("UsedDays.Invalid", "Employee cannot use more than allocated leave days");
        }

        public static class LeaveAllocation
        {
            public static readonly Error InstanceCreationFailed = Error.Failure("Instantion.Failed", "LeaveAllocation creation failed");
            public static readonly Error AllocationNotFound = Error.NotFound("Alocation.Unexistent", "No allocation was found with the same type of the request");
            public static readonly Error DuplicateAllocation = Error.Problem("Allocation.Duplicate", "One leave allocation per employee only");
        }

        public static class LeaveRequest
        {

            public static readonly Error InvalidRequestStatus = Error.Validation("Request.Invalid", "Leave request status was invalid");
            public static readonly Error RequestAlreadyExists = Error.Problem("Request.Duplicate", "Only one request per allocation");
            public static readonly Error OverLappingRequest = Error.Problem("Request.Overlap", "Leave request has overlapping dates");
            public static readonly Error InvalidDateRange = Error.Problem("Invalid.Dates", "Start date of request is past the end date");
            public static readonly Error InvalidEmployee = Error.Problem("Employee.Invalid", "Request employee id and this employee are different");
            public static readonly Error NullLeaveRequest = Error.Problem("Request.Null", "Passed leave request instance was null");
            public static readonly Error RequestNotFound = Error.NotFound("Request.Unexistent", "No request was found");
            public static readonly Error InvalidYear = Error.Validation("Year.Invalid", "The input year has already passed");
        }

        public static class Department
        {
            public static readonly Error NulllDepartment = Error.NotFound("Department.Null", "Passed department instance was null");
        }

        public static class LeaveType
        {
            public static readonly Error NullLeaveType = Error.Problem("LeaveType.Null", "Passed leave type instance was null");
            public static readonly Error EmptyLeaveName = Error.NotFound("LeaveType.Name.Empty", "Leave type name is empty");
        }
        public static class RefreshToken
        {
            public static readonly Error NullToken = Error.Problem("Token.Null", "Passed user instance was null");
            public static readonly Error TokenExpired = Error.Failure("Token.Expired", "Refresh token was not expired");
            public static readonly Error RevokedToken = Error.Failure("Token.Revoked", "Refresh token is invalid");
        }
    }
}
