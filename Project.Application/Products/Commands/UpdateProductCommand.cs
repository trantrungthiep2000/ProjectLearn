using MediatR;
using Project.Application.Models;

namespace Project.Application.Products.Commands;

/// <summary>
/// Information of update product command
/// CreatedBy: ThiepTT(08/11/2023)
/// </summary>
public class UpdateProductCommand : IRequest<OperationResult<string>>
{
    /// <summary>
    /// Id of product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Name of product
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Updated by
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;
}