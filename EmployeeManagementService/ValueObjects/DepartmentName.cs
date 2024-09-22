using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMgmt.Domain.ValueObjects
{
    public class DepartmentName
    {
        public string Value { get; private set; }

        public DepartmentName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Department name cannot be empty.");

            if (value.Length > 50)
                throw new ArgumentException("Department name cannot exceed 50 characters.");

            Value = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return ((DepartmentName)obj).Value == Value;
        }

        public override int GetHashCode() => Value.GetHashCode();
    }
}

