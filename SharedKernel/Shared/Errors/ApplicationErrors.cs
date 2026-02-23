using SharedKernel.Shared.Result;

namespace SharedKernel.Shared.Errors
{
    public static class ApplicationErrors
    {
        public static class Employee
        {
            public static Error EmployeeNotFound(Guid id) => Error.NotFound("Employee.NotFound", $"Employee with the id: {id} was not found");
            public static readonly Error NoEmployeesFound = Error.NotFound("Employees.NotFound", "There are no registered employees");
            public static readonly Error InvalidToken = Error.NotFound("Token.Invalid", "Token was invalid");
            public static readonly Error AlreadyVerified = Error.Validation("Email.Verified", "The user Email is already verified");
        }

        public static class General
        {
            public static readonly Error InternalError = Error.Failure("General.InternalError", "An internal error occurred. Please try again later.");
        }

        public static class LeaveType
        {
            public static Error LeaveTypeNotFound(Guid id) => Error.NotFound("LeaveType.NotFound", $"Leave type with the given id: {id} was not found");
            public static readonly Error NoLeaveFound = Error.NotFound("Leave.NotFound", "There are no found leaves");
        }

        public static class LeaveRequests 
        {
            public static Error RequestNotFound(Guid id) => Error.NotFound("Requests.NotFound", $"There are no found leave requests with the id: {id}");
            public static readonly Error NoRequestsFound = Error.NotFound("Requests.NotFound", "There are no found leave requests");
        }

        public static class LeaveAllocation
        {
            public static Error LeaveAllocationNotFound(Guid id) => Error.NotFound("LeaveAllocation.NotFound", $"Leave allocation with the given id: {id} was not found");
            public static readonly Error NoAllocationsFound = Error.NotFound("Allocation.NotFound", "There are no found allocations");
        }
        public static class Email
        {
            public static readonly Error EmailInvalid = Error.Validation("Email.Invalid", "Email is invalid");
        }
    }
}