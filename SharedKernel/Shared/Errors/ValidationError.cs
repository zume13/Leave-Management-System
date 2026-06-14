using SharedKernel.Shared.Result;

namespace SharedKernel.Shared.Errors
{
    public class ValidationError : Error
    {
        public Error[] Errors { get; }

        public ValidationError(Error[] errors)
            : base(
                "GeneralValidation.Error",
                string.Join("; ", errors.Select(e =>
                    $"{e.Code}: {e.Description}")),
                ErrorType.Validation)
        {
            Errors = errors;
        }
    }
}