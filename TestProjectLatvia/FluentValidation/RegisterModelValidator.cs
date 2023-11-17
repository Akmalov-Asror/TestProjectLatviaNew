using FluentValidation;
using TestProjectLatvia.ExtensionFunctions;
using TestProjectLatvia.ViewModels;

namespace TestProjectLatvia.FluentValidation;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    public RegisterModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .Must(CheckEmail.HaveCapitalLetter).WithMessage("Password must contain at least one capital letter.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required.")
            .Equal(x => x.Password).WithMessage("Passwords do not match.")
            .Must(CheckEmail.HaveCapitalLetter).WithMessage("Password must contain at least one capital letter.");

    }
}