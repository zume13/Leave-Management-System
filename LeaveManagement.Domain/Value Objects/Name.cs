using LeaveManagement.Domain.Primitives;
using SharedKernel.Shared.Errors;
using SharedKernel.Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Value_Objects
{
    public class Name : ValueObject
    {
        public string Value { get; init; }
        public const int MaxLength = 50;
        protected Name(string value) 
        { 
            Value = value;     
        }
        
        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static ResultT<Name> Create(string value)
        {

            if (string.IsNullOrWhiteSpace(value))
                return DomainErrors.General.EmptyName;

            value = value.Trim();

            if (value.Length > MaxLength)
                return DomainErrors.General.StringTooLong;

            return ResultT<Name>.Success(new Name(value));
        }
    }
}
