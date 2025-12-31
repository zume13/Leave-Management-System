using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Shared
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
