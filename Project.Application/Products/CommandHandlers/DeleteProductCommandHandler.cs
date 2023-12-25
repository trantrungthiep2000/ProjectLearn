using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Messages;
using Project.Application.Models;
using Project.Application.Products.Commands;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IRepositories;
using Project.Domain.Interfaces.IServices;

namespace Project.Application.Products.CommandHandlers;

/// <summary>
/// Information of delete product command handler
/// CreatedBy: ThiepTT(08/11/2023)
/// </summary>
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, OperationResult<string>>
{
    private readonly IProductRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IProductRepository<Product> productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">DeleteProductCommand</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Message deleted product</returns>
    /// CreatedBy: ThiepTT(08/11/2023)
    public async Task<OperationResult<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
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

            _productRepository.Delete(product);

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
}