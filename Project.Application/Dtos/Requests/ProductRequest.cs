namespace Project.Application.Dtos.Requests;

/// <summary>
/// Information of product request
/// CreatedBy: ThiepTT(08/11/2023)
/// </summary>
public class ProductRequest 
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
}