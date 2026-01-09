using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static class  LeaveType
        {
            public static readonly Error LeaveTypeNotFound = new Error("LeaveType.NotFound", "Leave type with the given id was not found");
        }
    }
}
