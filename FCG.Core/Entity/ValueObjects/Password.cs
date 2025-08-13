using System;
using System.Security.Cryptography;
using System.Text;

namespace FCG.Core.Entity.ValueObjects
{
    public class Password
    {
        public string Value { get; }

        private Password(string hashedValue)
        {
            Value = hashedValue;
        }

        public static Password Create(string plainTextPassword)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword))
                throw new ArgumentNullException(nameof(plainTextPassword));

            if (plainTextPassword.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long.");

            var hashedPassword = HashPassword(plainTextPassword);
            return new Password(hashedPassword);
        }

        public static Password FromHashedValue(string hashedValue)
        {
            if (string.IsNullOrWhiteSpace(hashedValue))
                throw new ArgumentNullException(nameof(hashedValue));

            return new Password(hashedValue);
        }

        public bool VerifyPassword(string plainTextPassword)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword))
                return false;

            var hashedInput = HashPassword(plainTextPassword);
            return Value.Equals(hashedInput);
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static implicit operator string(Password password) => password.Value;
    }
}