using FluentValidation;
using TestProjectLatvia.ExtensionFunctions;
using TestProjectLatvia.ViewModels;

namespace TestProjectLatvia.FluentValidation;
public class LoginModelValidator : AbstractValidator<LoginModel>
{
    /// <summary>
    /// Initializes a new instance of the LoginModelValidator class.
    /// </summary>
    public LoginModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
            .Must(CheckEmail.HaveCapitalLetter).WithMessage("Password must contain at least one capital letter.");
    }
}