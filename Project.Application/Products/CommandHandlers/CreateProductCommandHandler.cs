using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Messages;
using Project.Application.Models;
using Project.Application.Products.Commands;
using Project.Application.Validators;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IRepositories;
using Project.Domain.Interfaces.IServices;

namespace Project.Application.Products.CommandHandlers;

/// <summary>
/// Information of create product command handler
/// CreatedBy: ThiepTT(08/11/2023)
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, OperationResult<string>>
{
    private readonly IProductRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">CreateProductCommand</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Message created product</returns>
    /// CreatedBy: ThiepTT(08/11/2023)
    public async Task<OperationResult<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new OperationResult<string>();

        await using IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Create product
            Product product = Product.CreateProduct(request.ProductName, request.Price, request.Description, request.CreatedBy);

            // Validate request of product
            if (ValidateProduct(product, result)) { return result; }

            _productRepository.Create(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            result.Data = ResponseMessage.Product.CreateProductSuccess;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }

    /// <summary>
    /// Validate product
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="result">OperationResult<string></param>
    /// <returns>True | False</returns>
    /// CreatedBy: ThiepTT(08/11/2023)
    private bool ValidateProduct(Product product, OperationResult<string> result)
    {
        ProductValidator validator = new ProductValidator();

        ValidationResult validationRegister = validator.Validate(product);

        if (!validationRegister.IsValid)
        {
            foreach (ValidationFailure error in validationRegister.Errors)
            {
                result.AddError(ErrorCode.BadRequest, error.ErrorMessage);
            }

            return true;
        }

        return false;
    }
}