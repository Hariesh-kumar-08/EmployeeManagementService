using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMgmt.Domain.ValueObjects
{
    public class EmailAddress
    {
        public string Value { get; private set; }

        public EmailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty.");

            if (!new EmailAddressAttribute().IsValid(value))
                throw new ArgumentException("Invalid email format.");

            Value = value;
        }
        public override string ToString() => Value;
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return ((EmailAddress)obj).Value == Value;
        }

        public override int GetHashCode() => Value.GetHashCode();
    }
}

