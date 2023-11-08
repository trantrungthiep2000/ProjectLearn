using FluentValidation;
using Project.Domain.Aggregates;

namespace Project.Application.Validators;

/// <summary>
/// Information of product validator
/// CreatedBy: ThiepTT(30/10/2023)
/// </summary>
public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(product => product.ProductName)
            .NotNull().WithMessage("Product name cannot be empty")
            .NotEmpty().WithMessage("Product name cannot be empty")
            .MinimumLength(1).WithMessage("Product name must be at least 1 character long")
            .MaximumLength(50).WithMessage("Product name can contaimm at most 50 charactor");

        RuleFor(product => product.Price)
            .NotNull().WithMessage("Price cannot be empty")
            .NotEmpty().WithMessage("Price cannot be empty");

        RuleFor(product => product.Description)
          .NotNull().WithMessage("Description cannot be empty")
          .NotEmpty().WithMessage("Description cannot be empty");
    }
}