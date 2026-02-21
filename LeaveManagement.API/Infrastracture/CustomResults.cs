using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Infrastracture
{
    public static class CustomResults
    {
        public static IActionResult Problem(Result result)
        {
            if(!result.isFailure)
            {
                throw new ArgumentException("Result must be a failure.", nameof(result));
            }

            return Results.Problem(
                title: GetTitle(result.Error),
                detail: )

            static string GetTitle(Error error)
            {
                return String.Empty;
            }
        }
    }
}
