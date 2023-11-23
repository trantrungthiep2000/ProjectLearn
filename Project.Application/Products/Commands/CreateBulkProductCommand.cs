using MediatR;
using Microsoft.AspNetCore.Http;
using Project.Application.Models;

namespace Project.Application.Products.Commands;

/// <summary>
/// Information of create bulk product command
/// CreatedBy: ThiepTT(10/11/2023)
/// </summary>
public class CreateBulkProductCommand : IRequest<OperationResult<string>>
{
    /// <summary>
    /// File
    /// </summary>
    public IFormFile? File { get; set; }

    /// <summary>
    /// Created  by
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
}