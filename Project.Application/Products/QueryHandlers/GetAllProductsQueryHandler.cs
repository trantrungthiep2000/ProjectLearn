using MediatR;
using Project.Application.Models;
using Project.Application.Products.Queries;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IRepositories;

namespace Project.Application.Products.QueryHandlers;

/// <summary>
/// Information of get all products query handler
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, OperationResult<IEnumerable<Product>>>
{
    private readonly IProductRepository<Product> _productRepository;

    public GetAllProductsQueryHandler(IProductRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">GetAllProductsQuery</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>List of product</returns>
    ///  CreatedBy: ThiepTT(07/11/2023)
    public async Task<OperationResult<IEnumerable<Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        OperationResult<IEnumerable<Product>> result = new OperationResult<IEnumerable<Product>>();

        try
        {
            IEnumerable<Product> products = await _productRepository.GetAllAsync();

            result.Data = products;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }
}