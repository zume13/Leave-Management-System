namespace SharedKernel.Shared.Result
{
    public class Error
    {
        public string Code { get; set; }
        public string? Description { get; set; }

        public Error(string code, string desc) 
        {
            Code = code;
            Description = desc;
        }

        public static readonly Error None = new("No error", string.Empty);

        public static implicit operator Result(Error error) => Result.Failure(error);
    }
}
