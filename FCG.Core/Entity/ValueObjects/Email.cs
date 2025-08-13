using System;
using System.Text.RegularExpressions;

namespace FCG.Core.Entity.ValueObjects
{
    public class Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            email = email.Trim();

            if (email.Length > 100)
                throw new ArgumentException("Email too long. Maximum 100 characters.", nameof(email));

            if (!IsValidEmail(email))
                throw new ArgumentException("Email format invalid", nameof(email));

            return new Email(email);
        }

        private static bool IsValidEmail(string email)
        {
            // Regex to email validation
            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email);
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(Email email) => email.Value;
    }
}