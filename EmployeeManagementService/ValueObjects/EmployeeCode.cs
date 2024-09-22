using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMgmt.Domain.ValueObjects
{
    public class EmployeeCode
    {
        public string Value { get; private set; }

        public EmployeeCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Employee code cannot be empty.");

            if (value.Length > 10)
                throw new ArgumentException("Employee code cannot exceed 10 characters.");

            Value = value;
        }
        public override string ToString() => Value;
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return ((EmployeeCode)obj).Value == Value;
        }

        public override int GetHashCode() => Value.GetHashCode();
    }
}
