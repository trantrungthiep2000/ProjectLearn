using FluentValidation;
using Project.Domain.Aggregates;

namespace Project.Application.Validators;

/// <summary>
/// Information of user profile validator
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class UserProfileValidator : AbstractValidator<UserProfile>
{
    public UserProfileValidator()
    {
        RuleFor(userProfile => userProfile.FullName)
            .NotNull().WithMessage("Full name cannot be empty")
            .NotEmpty().WithMessage("Full name cannot be empty")
            .MinimumLength(1).WithMessage("Full name must be at least 1 character long")
            .MaximumLength(50).WithMessage("Full name can contaimm at most 50 charactor");

        RuleFor(userProfile => userProfile.Email)
            .NotNull().WithMessage("Email cannot be empty")
            .NotEmpty().WithMessage("Email cannot be empty")
            .MinimumLength(1).WithMessage("Email must be at least 1 character long")
            .MaximumLength(255).WithMessage("Email can contaimm at most 255 charactor")
            .EmailAddress().WithMessage("Email is in wrong format");

        RuleFor(userProfile => userProfile.PhoneNumber)
            .NotNull().WithMessage("Phone numner cannot be empty")
            .NotEmpty().WithMessage("Phone number cannot be empty")
            .MinimumLength(10).WithMessage("Phone number must be at least 10 character long")
            .MaximumLength(20).WithMessage("Phone number can contaimm at most 20 charactor");

        RuleFor(userProfile => userProfile.DateOfBirth)
            .NotNull().WithMessage("Date of birth cannot be empty")
            .NotEmpty().WithMessage("Date of birth cannot be empty");
    }
}