
namespace SharedKernel.Shared
{
    public static class ApplicationErrors
    {
        public static class Employee
        {
            public static readonly Error EmployeeNotFound = new Error("Employee.NotFound", "Employee with the id was not found");
            public static readonly Error NoEmployeesFound = new Error("Employees.NotFound", "There are no registered employees");
        }

        public static class General
        {
            public static readonly Error InternalError = new Error("General.InternalError", "An internal error occurred. Please try again later.");
        }

        public static class LeaveType
        {
            public static readonly Error LeaveTypeNotFound = new Error("LeaveType.NotFound", "Leave type with the given id was not found");
            public static readonly Error NoLeaveFound = new Error("Leave.NotFound", "There are no found leaves");
        }

        public static class LeaveRequests 
        {
            public static readonly Error NoRequestsFound = new Error("Requests.NotFound", "There are no found leave requests");
        }

        public static class LeaveAllocation
        {
            public static readonly Error LeaveAllocationNotFound = new Error("LeaveAllocation.NotFound", "Leave allocation with the given id was not found");
            public static readonly Error NoAllocationsFound = new Error("Allocation.NotFound", "There are no found allocations");
        }
    }
}