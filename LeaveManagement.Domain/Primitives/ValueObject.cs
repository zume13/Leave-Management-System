using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace LeaveManagement.Domain.Primitives
{
    public abstract class ValueObject
    {

        public abstract IEnumerable<object> GetAtomicValues();

        public bool Equal(ValueObject other)
        {

            return other is ValueObject && ValuesAreEqual(other);

        }

        public bool Equals(ValueObject value)
        {

            return value is not null && ValuesAreEqual(value);

        }

        public override bool Equals(object? obj)
        {

            return obj is ValueObject other && ValuesAreEqual(other);

        }

        public override int GetHashCode()
        {

            return GetAtomicValues()
                .Aggregate(
                default(int),
                HashCode.Combine);

        }

        private bool ValuesAreEqual(ValueObject other)
        {

            return GetAtomicValues().SequenceEqual(other.GetAtomicValues());

        }

    }
}
