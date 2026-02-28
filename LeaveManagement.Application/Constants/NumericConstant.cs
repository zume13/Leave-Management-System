
namespace LeaveManagement.Application.Constants
{
    public static class NumericConstant
    {
        public static int MaxPageSize(int pageSize) => Math.Min(pageSize, 30);
        public const int DefaultPageSize = 20;
    }
}
