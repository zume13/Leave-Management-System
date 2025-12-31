using LeaveManagement.Domain.Primitives;
using LeaveManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Value_Objects
{
    public class Email : ValueObject
    {
        public string Value { get; init; }
        public const int MaxLength = 50;

        protected Email(string value)
        {
            Value = value;
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static ResultT<Email> Create(string value)
        {
            if(!value.Contains('@'))
                return DomainErrors.Email.EmailInvalid;
            
            value = value.Trim();

            if(string.IsNullOrWhiteSpace(value))
                return DomainErrors.Email.EmptyEmail;

            if (value.Length > MaxLength)
                return DomainErrors.General.StringTooLong;

            return ResultT<Email>.Success(new Email(value));

        }
    }
}
