using MediatR;
using Project.Application.Models;
using Project.Domain.Aggregates;

namespace Project.Application.Products.Queries;

/// <summary>
/// Information of get all products query
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
public class GetAllProductsQuery : IRequest<OperationResult<IEnumerable<Product>>> { }