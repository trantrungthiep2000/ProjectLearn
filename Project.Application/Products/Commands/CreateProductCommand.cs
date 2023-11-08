using MediatR;
using Project.Application.Models;

namespace Project.Application.Products.Commands;

/// <summary>
/// Information of create product command
/// CreatedBy: ThiepTT(08/11/2023)
/// </summary>
public class CreateProductCommand : IRequest<OperationResult<string>>
{
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
    /// Created by
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
}