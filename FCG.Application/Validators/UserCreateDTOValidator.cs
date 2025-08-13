using FluentValidation;
using FCG.Application.DTOs;

namespace FCG.Application.Validators
{
    public class UserCreateDTOValidator : AbstractValidator<UserCreateDTO>
    {
        public UserCreateDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email must be a valid email address.")
                .MaximumLength(255)
                .WithMessage("Email cannot exceed 255 characters.");

            RuleFor(x => x.Password)
                .SetValidator(new PasswordValidator());

            RuleFor(x => x.RoleId)
                .GreaterThan(0)
                .WithMessage("Role ID must be greater than 0.");
        }
    }
}