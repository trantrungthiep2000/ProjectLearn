using MediatR;
using Project.Application.Models;

namespace Project.Application.Products.Commands;

/// <summary>
/// Information of delete product command
/// CreatedBy: ThiepTT(08/11/2023)
/// </summary>
public class DeleteProductCommand : IRequest<OperationResult<string>>
{
    /// <summary>
    /// Id of product
    /// </summary>
    public Guid ProductId { get; set; }
}