
namespace SharedKernel.Shared
{
    public class Result
    {
        public bool isSuccess { get; }
        public bool isFailure => !isSuccess;
        public Error Error { get; }

        protected internal Result(bool isSuccess, Error error) 
        { 
            if(isSuccess && error != Error.None || 
                !isSuccess && error == Error.None)
                throw new ArgumentException("Invalid Error", nameof(error));

            this.isSuccess = isSuccess;
            this.Error = error;
        }

        public static Result Success() => new(true, Error.None);

        public static Result Failure(Error error) => new(false, error);

    }
}
