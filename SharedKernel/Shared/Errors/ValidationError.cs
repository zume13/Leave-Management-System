using SharedKernel.Shared.Result;

namespace SharedKernel.Shared.Errors
{
    public class ValidationError(Error[] errors) : Error("GeneralValidation.Error", "Error/s had occured during validation", ErrorType.Validation)
    {
        public Error[] Errors { get; set; } = errors;
    }
}
