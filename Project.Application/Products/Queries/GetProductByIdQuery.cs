using MediatR;
using Project.Application.Models;
using Project.Domain.Aggregates;

namespace Project.Application.Products.Queries;

/// <summary>
/// Information of get product by id query
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
public class GetProductByIdQuery : IRequest<OperationResult<Product>> 
{
    /// <summary>
    /// Id of product
    /// </summary>
    public Guid ProductId { get; set; }
}