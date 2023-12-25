using MediatR;
using Project.Application.Messages;
using Project.Application.Models;
using Project.Application.Products.Queries;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IRepositories;

namespace Project.Application.Products.QueryHandlers;

/// <summary>
/// Inforamation of get product by id query handler
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, OperationResult<Product>>
{
    private readonly IProductRepository<Product> _productRepository;

    public GetProductByIdQueryHandler(IProductRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">GetProductByIdQuery</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Information of product</returns>
    /// CreatedBy: ThiepTT(07/11/2023)
    public async Task<OperationResult<Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        OperationResult<Product> result = new OperationResult<Product>();

        try
        {
            Product productById = await _productRepository.GetByIdAsync(request.ProductId);

            if (productById is null)
            {
                result.AddError(ErrorCode.NotFound, string.Format(ErrorMessage.Product.ProductNotFound, request.ProductId));
                return result;
            }

            result.Data = productById;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }
}