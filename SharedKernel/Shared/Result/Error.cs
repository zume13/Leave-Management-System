namespace SharedKernel.Shared.Result
{
    public class Error
    {
        public string Code { get; set; }
        public string? Description { get; set; }
        public ErrorType Type { get; set; } 

        public Error(string code, string desc, ErrorType type) 
        {
            Code = code;
            Description = desc;
            Type = type;
        }

        public static readonly Error None = new("No error", string.Empty, ErrorType.None);

        public static implicit operator Result(Error error) => Result.Failure(error);

        public static Error Problem(string code, string desc) => new(code, desc, ErrorType.Problem);
        public static Error Validation(string code, string desc) => new(code, desc, ErrorType.Validation);
        public static Error Failure(string code, string desc) => new(code, desc, ErrorType.Failure);
        public static Error NotFound(string code, string desc) => new(code, desc, ErrorType.NotFound);
    }
}
