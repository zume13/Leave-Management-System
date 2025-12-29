using LeaveManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Commons.Shared
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
