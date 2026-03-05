namespace LeaveManagement.API.Constants
{
    public static class Auth
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Employee = "Employee";
            public const string Manager = "Manager";    
        }

        public static class Policies
        {
            public const string AdminOnly = "AdminOnly";
            public const string Employee = "Employee";
            public const string Manager = "Manager";
            public const string CanRequestLeave = "CanRequestLeave";
            public const string CanApproveLeave = "CanApproveLeave";
        }
    }
}
