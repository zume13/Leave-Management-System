using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared.Result;

namespace LeaveManagement.API.Infrastructure
{
    public static class CustomResults
    {
        public static IActionResult Problem(Result result)
        {
            if (!result.isFailure)
            {
                throw new ArgumentException("Result must be a failure.", nameof(result));
            }

            var statusCode = GetStatusCode(result.Error);

            var problemDetails = new ProblemDetails
            {
                Title = GetTitle(result.Error),
                Detail = result.Error.Description,
                Status = statusCode,
                Type = GetType(result.Error)
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
        }

        private static string GetTitle(Error error) =>
            error.Type switch
            {
                ErrorType.Validation => "Validation Error",
                ErrorType.NotFound => "Resource Not Found",
                ErrorType.Problem => "Conflict",
                _ => "Server Error"
            };

        private static int GetStatusCode(Error error) =>
            error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Problem => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

        private static string GetType(Error error) =>
            error.Type switch
            {
                ErrorType.Validation => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                ErrorType.Problem => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
                _ => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            };
    }
}