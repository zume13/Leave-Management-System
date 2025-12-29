using LeaveManagement.Domain.Commons.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Shared
{
    public class ResultT<TValue> : Result
    {

        private readonly TValue? _value;
        protected internal ResultT(TValue? value, bool isSuccess, Error error) 
            : base(isSuccess, error) => 
            _value = value;

        public TValue Value => isSuccess ? _value! : throw new InvalidOperationException("Cannot access value of a failure result");

        public static ResultT<TValue> Success(TValue value) => new(value, true, Error.None);

        public static new ResultT<TValue> Failure(Error error) => new(default, false, error);

        public static implicit operator ResultT<TValue>(Error error) => Failure(error);
    }
}
