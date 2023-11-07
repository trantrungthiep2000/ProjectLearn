namespace Project.Application.Dtos.Responses;

/// <summary>
/// Information of product response
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
public class ProductResponse
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
}