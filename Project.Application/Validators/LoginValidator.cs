using FluentValidation;
using Project.Application.Identities.Commands;

namespace Project.Application.Validators;

/// <summary>
/// Information of login validator
/// CreatedBy: ThiepTT(01/11/2023)
/// </summary>
public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(login => login.Email)
            .NotNull().WithMessage("Email cannot be empty")
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Email is in wrong format");

        RuleFor(login => login.Password)
            .NotNull().WithMessage("Password cannot be empty")
            .NotEmpty().WithMessage("Password cannot be empty");
    }
}