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
/// Information of update product command handler
/// CreatedBy: ThiepTT(08/11/2023)
/// </summary>
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, OperationResult<string>>
{
    private readonly IProductRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IProductRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">UpdateProductCommand</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Message updated product</returns>
    /// CreatedBy: ThiepTT(08/11/2023)
    public async Task<OperationResult<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new OperationResult<string>();

        await using IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            Product product = await _productRepository.GetByIdAsync(request.ProductId);  
            
            if (product is null)
            {
                result.AddError(ErrorCode.NotFound, string.Format(ErrorMessage.Product.ProductNotFound, request.ProductId));

                return result;
            }

            product.UpdateProduct(request.ProductName, request.Price, request.Description, request.UpdatedBy);

            // Validate request of product
            if (ValidateProduct(product, result)) { return result; }

            _productRepository.Update(product);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            result.Data = ResponseMessage.Product.UpdateProductSuccess;
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