using MediatR;
using Project.Application.Models;

namespace Project.Application.Products.Commands;

/// <summary>
/// Information of delete bulk product command
/// CreatedBy: ThiepTT(10/11/2023)
/// </summary>
public class DeleteBulkProductCommand : IRequest<OperationResult<string>>
{
    /// <summary>
    /// list id of product
    /// </summary>
    public List<string> ListProductId { get; set; } = new List<string>();
}