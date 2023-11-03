using FluentValidation;
using Project.Application.Users.Commands;

namespace Project.Application.Validators;

/// <summary>
/// Information of update user profile validator
/// CreatedBY: ThiepTT(03/11/2023)
/// </summary>
public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(userProfile => userProfile.FullName)
           .NotNull().WithMessage("Full name cannot be empty")
           .NotEmpty().WithMessage("Full name cannot be empty")
           .MinimumLength(1).WithMessage("Full name must be at least 1 character long")
           .MaximumLength(50).WithMessage("Full name can contaimm at most 50 charactor");

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