using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Messages;
using Project.Application.Models;
using Project.Application.Products.Commands;
using Project.Domain.Aggregates;
using Project.Infrastructure.Interfaces;

namespace Project.Application.Products.CommandHandlers;

/// <summary>
/// Information of delete bulk product command handler
/// CreatedBy: ThiepTT(10/11/2023)
/// </summary>
public class DeleteBulkProductCommandHandler : IRequestHandler<DeleteBulkProductCommand, OperationResult<string>>
{
    private readonly IProductRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBulkProductCommandHandler(IProductRepository<Product> productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">DeleteBulkProductCommand</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Message delete success</returns>
    /// CreatedBy: ThiepTT(10/11/2023)
    public async Task<OperationResult<string>> Handle(DeleteBulkProductCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new OperationResult<string>();

        await using IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            if (request.ListProductId is null || request.ListProductId.Count <= 0)
            {
                result.AddError(ErrorCode.NotFound, ErrorMessage.Product.ProductNotEmpty);
                return result;
            }

            List<Product> products = await GetAllProductsDeleteAsync(request.ListProductId, result);

            if (result.IsError) { return result; }

            _productRepository.DeleteBulk(products);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            result.Data = ResponseMessage.Product.DeleteProductSuccess;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }

    /// <summary>
    /// Get all products delete async
    /// </summary>
    /// <param name="listProductId">List id of product</param>
    /// <param name="result">OperationResult<string></param>
    /// <returns>List product</returns>
    /// CreatedBy: ThiepTT(10/11/2023)
    private async Task<List<Product>> GetAllProductsDeleteAsync(List<string> listProductId, OperationResult<string> result)
    {
        List<Product> products = new List<Product>();

        foreach (string productId in listProductId)
        {
            Guid.TryParse(productId, out Guid id);

            if (id == default(Guid))
            {
                result.AddError(ErrorCode.NotFound, string.Format(ErrorMessage.Product.ProductNotFound, productId));
                return new List<Product>();
            }

            Product product = await _productRepository.GetByIdAsync(id);

            if (product is null)
            {
                result.AddError(ErrorCode.NotFound, string.Format(ErrorMessage.Product.ProductNotFound, productId));
                return new List<Product>();
            }

            products.Add(product);
        }

        return products;
    }
}