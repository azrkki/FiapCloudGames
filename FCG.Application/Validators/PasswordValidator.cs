using FluentValidation;
using System.Text.RegularExpressions;

namespace FCG.Application.Validators
{
    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            RuleFor(password => password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .Must(ContainLowercase)
                .WithMessage("Password must contain at least one lowercase letter.")
                .Must(ContainUppercase)
                .WithMessage("Password must contain at least one uppercase letter.")
                .Must(ContainDigit)
                .WithMessage("Password must contain at least one number.")
                .Must(ContainSpecialCharacter)
                .WithMessage("Password must contain at least one special character (!@#$%^&*()_+-=[]{}|;:,.<>?).");
        }

        private bool ContainLowercase(string password)
        {
            return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"[a-z]");
        }

        private bool ContainUppercase(string password)
        {
            return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"[A-Z]");
        }

        private bool ContainDigit(string password)
        {
        return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"[0-9]");
        }

        private bool ContainSpecialCharacter(string password)
        {
            return !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]");
        }
    }
}