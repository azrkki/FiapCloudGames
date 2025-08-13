using FluentValidation;
using FCG.Application.DTOs;

namespace FCG.Application.Validators
{
    public class UserPasswordUpdateDTOValidator : AbstractValidator<UserPasswordUpdateDTO>
    {
        public UserPasswordUpdateDTOValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("User ID must be greater than 0.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .SetValidator(new PasswordValidator())
                .Must((dto, newPassword) => newPassword != dto.CurrentPassword)
                .WithMessage("New password must be different from the current password.");
        }
    }
}